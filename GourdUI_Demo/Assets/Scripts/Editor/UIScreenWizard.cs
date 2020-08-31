using System;
using System.IO;
using System.Reflection;
using System.Text;
using GourdUI;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace Editor
{
    public static class UIScreenWizard
    {
        public static void OnCreateNewUIScreen(string screenName)
        {
            EditorPrefs.SetString ("ScreenName", screenName);
            
            // Create project directory...
            EditorUtility.DisplayProgressBar(
                "Creating UIScreen Structure...", 
                "Directories", 0.2f);
            Tuple<string,string,string,string> directoryPaths =
                CreateScreenDirectoryStructure(screenName);
            
            // Create project data items...
            EditorUtility.DisplayProgressBar(
                "Creating UIScreen Structure...", 
                "Data Assets", 0.4f);
            Tuple<UIScreenConfigDataTemplate,UIViewConfigDataTemplate> dataObjects = 
                CreateScreenDataObjects(screenName);
            
            // Create scripts
            EditorUtility.DisplayProgressBar(
                "Creating UIScreen Structure...", 
                "Compiling Scripts...", 0.6f);
            Tuple<string, string> newtypes = 
                CreateScreenScripts(directoryPaths.Item3, screenName);
            EditorPrefs.SetString ("ScreenClassType", newtypes.Item1);
            EditorPrefs.SetString ("ViewClassType", newtypes.Item2);

            // Finished!
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            CompilationPipeline.RequestScriptCompilation();
            
            Selection.activeObject = dataObjects.Item1;
        }

        private static Tuple<string,string,string,string> 
            CreateScreenDirectoryStructure(string screenName)
        {
            // Standard data
            string appPath = Application.dataPath;
            string uiScreenRootPath = Path.Combine(appPath, "UI");
            uiScreenRootPath = Path.Combine(uiScreenRootPath, "Screens");
            
            // Create the root
            DirectoryInfo rootScreenPath = Directory.CreateDirectory(
                Path.Combine(uiScreenRootPath, screenName));
            // Create prefabs folder
            DirectoryInfo screenPrefabsPath = Directory.CreateDirectory(
                Path.Combine(rootScreenPath.FullName, "Prefabs"));
            // Create scripts folder
            DirectoryInfo screenScriptsPath = Directory.CreateDirectory(
                Path.Combine(rootScreenPath.FullName, "Scripts"));
            // Create views folder
            DirectoryInfo screenViewsPath = Directory.CreateDirectory(
                Path.Combine(rootScreenPath.FullName, "Views"));
            
            AssetDatabase.Refresh();
            return new Tuple<string, string, string, string>(
                rootScreenPath.FullName,
                screenPrefabsPath.FullName,
                screenScriptsPath.FullName,
                screenViewsPath.FullName);
        }

        private static Tuple<UIScreenConfigDataTemplate,UIViewConfigDataTemplate> 
            CreateScreenDataObjects(string screenName)
        {
            string pathHeader = "Assets/UI/Screens/" + screenName + "/";

            // Create view object data
            UIViewConfigDataTemplate viewDataObj =
                ScriptableObject.CreateInstance<UIViewConfigDataTemplate>();
            AssetDatabase.CreateAsset(
                viewDataObj, 
                pathHeader + "Views/UIView_" + screenName + "_Default_Data.asset");
            
            // Create screen object data
            UIScreenConfigDataTemplate screenDataObj =
                ScriptableObject.CreateInstance<UIScreenConfigDataTemplate>();
            
            // Assign view to screen
            screenDataObj.data.views = new[] {viewDataObj};
            screenDataObj.data.triggerCode = screenName.ToLower();
            AssetDatabase.CreateAsset(
                screenDataObj, 
                pathHeader + "UIScreen_" + screenName + "_Data.asset");
            
            // Save the assets
            return new Tuple<UIScreenConfigDataTemplate, UIViewConfigDataTemplate>(screenDataObj, viewDataObj);
        }

        private static Tuple<string,string> CreateScreenScripts(string scriptsPath,string screenName)
        {
            // Screen contract
            string contractScreenClass = "IUIContractScreen_" + screenName;
            using (FileStream fs = File.Create(Path.Combine(scriptsPath, contractScreenClass + ".cs")))
            {
                byte[] data = GetScriptTemplateContents(contractScreenClass, "GourdUI/IUIContractScreenTemplate");
                fs.Write(data, 0, data.Length);
            } 
            
            // View contract
            string contractViewClass = "IUIContractView_" + screenName;
            using (FileStream fs = File.Create(Path.Combine(scriptsPath, contractViewClass + ".cs")))
            {
                byte[] data = GetScriptTemplateContents(contractViewClass, "GourdUI/IUIContractViewTemplate");
                fs.Write(data, 0, data.Length);
            } 
            
            // UI state
            string stateClass = "UIState_" + screenName;
            using (FileStream fs = File.Create(Path.Combine(scriptsPath, stateClass + ".cs")))
            {
                byte[] data = GetScriptTemplateContents(stateClass, "GourdUI/UIStateTemplate");
                fs.Write(data, 0, data.Length);
            } 
            
            // UI screen
            string screenClass = "UIScreen_" + screenName;
            using (FileStream fs = File.Create(Path.Combine(scriptsPath, screenClass + ".cs")))
            {
                byte[] data = GetScreenScriptTemplateContents(
                    screenClass, 
                    "GourdUI/UIScreenTemplate",
                    contractScreenClass,
                    contractViewClass,
                    stateClass);
                fs.Write(data, 0, data.Length);
            } 

            // UI view
            string viewClass = "UIView_" + screenName + "_Default";
            using (FileStream fs = File.Create(Path.Combine(scriptsPath, viewClass + ".cs")))
            {
                byte[] data = GetViewScriptTemplateContents(
                    viewClass, 
                    "GourdUI/UIViewTemplate",
                    contractScreenClass,
                    contractViewClass,
                    screenClass);
                fs.Write(data, 0, data.Length);
            } 
            
            return new Tuple<string, string>(screenClass, viewClass);
        }

        private static byte[] GetScriptTemplateContents(string className, string templatePath)
        {
            string contents = Resources.Load<TextAsset>(templatePath).text;
            contents = contents.Replace("#CLASSNAME#", className);
            return new UTF8Encoding(true).GetBytes(contents);
        }
        
        private static byte[] GetScreenScriptTemplateContents(
            string className, 
            string templatePath,
            string screenContractClass,
            string viewContractClass,
            string stateClass)
        {
            string contents = Resources.Load<TextAsset>(templatePath).text;
            contents = contents.Replace("#CLASSNAME#", className);
            contents = contents.Replace("#CONTRACTSCREEN#", screenContractClass);
            contents = contents.Replace("#CONTRACTVIEW#", viewContractClass);
            contents = contents.Replace("#STATE#", stateClass);

            return new UTF8Encoding(true).GetBytes(contents);
        }
        
        private static byte[] GetViewScriptTemplateContents(
            string className, 
            string templatePath,
            string screenContractClass,
            string viewContractClass,
            string screenClass)
        {
            string contents = Resources.Load<TextAsset>(templatePath).text;
            contents = contents.Replace("#CLASSNAME#", className);
            contents = contents.Replace("#CONTRACTSCREEN#", screenContractClass);
            contents = contents.Replace("#CONTRACTVIEW#", viewContractClass);
            contents = contents.Replace("#SCREEN#", screenClass);

            return new UTF8Encoding(true).GetBytes(contents);
        }

        private static void CreateScreenPrefabs()
        {
            string n = EditorPrefs.GetString("ScreenName");
            
            // Create screen object prefab
            GameObject screenPrefabObj = new GameObject {name = "UIScreen_" + n};
            Type screenType = Type.GetType (EditorPrefs.GetString("ScreenClassType"),
                true);
            screenPrefabObj.AddComponent(screenType);
            
            // Set data field
            UIScreenConfigDataTemplate screenData =
                (UIScreenConfigDataTemplate) AssetDatabase.LoadAssetAtPath(
                    "Assets/UI/Screens/" + n + "/UIScreen_" + n + "_Data.asset",
                    typeof(UIScreenConfigDataTemplate));
            
            Component[] components = screenPrefabObj.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                Component co = components[i];
                if (co != null)
                {
                    Type t = co.GetType();
                    if (t == screenType)
                    {
                        System.Reflection.FieldInfo[] fieldInfo = t.GetFields();
                        foreach (System.Reflection.FieldInfo info in fieldInfo)
                        {
                            if (info.Name == "_configBaseData")
                            {
                                info.SetValue(co, screenData);
                                Debug.Log("FOUND");
                            }
                        }
                    }
                }
            }
            
            // Save prefab
            string path = "Assets/UI/Screens/" + n + "/Prefabs/UIScreen_" + n + ".prefab";
            PrefabUtility.SaveAsPrefabAssetAndConnect(
                screenPrefabObj, path, InteractionMode.UserAction);
            
            
            // Create view object prefab
            GameObject viewPrefabObj = new GameObject {name = "UIView_" + n + "_Default"};
            Type viewType = Type.GetType (EditorPrefs.GetString("ViewClassType"),
                true);
            Canvas c = viewPrefabObj.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler cs = viewPrefabObj.AddComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            viewPrefabObj.AddComponent<GraphicRaycaster>();
            viewPrefabObj.AddComponent(viewType);
            string viewPath = "Assets/UI/Screens/" + n + "/Prefabs/UIView_" + n + "_Default.prefab";
            GameObject viewPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(
                viewPrefabObj, viewPath, InteractionMode.UserAction);
            UnityEditor.Editor.DestroyImmediate(viewPrefabObj);
            
            // Assign view prefab to view data object
            UIViewConfigDataTemplate viewData =
                (UIViewConfigDataTemplate) AssetDatabase.LoadAssetAtPath(
                    "Assets/UI/Screens/" + n + "/Views/UIView_" + n + "_Default_Data.asset",
                    typeof(UIViewConfigDataTemplate));
            viewData.data.prefab = viewPrefab;
        }


        #region Utility

        public static object GetValue(this SerializedProperty property)
        {
            System.Type parentType = property.serializedObject.targetObject.GetType();
            System.Reflection.FieldInfo fi = parentType.GetField(property.propertyPath);  
            return fi.GetValue(property.serializedObject.targetObject);
        }
        
        public static void SetValue(this SerializedProperty property, Type t, object value)
        {
            System.Reflection.PropertyInfo fi = t.GetProperty(property.propertyPath);//this FieldInfo contains the type.
            Debug.Log(fi.Name);
            fi.SetValue(property.serializedObject.targetObject, value);
        }

        #endregion Utility
        
        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void ScriptReloaded()
        {
            if (EditorPrefs.HasKey("ScreenClassType"))
            {
                EditorUtility.DisplayProgressBar(
                    "Creating UIScreen Structure...", 
                    "Creating Prefabs", 0.8f);
                CreateScreenPrefabs();
                
                EditorPrefs.DeleteKey("ScreenClassType");
                EditorPrefs.DeleteKey("ViewClassType");
                EditorPrefs.DeleteKey("ScreenName");
                
                EditorUtility.ClearProgressBar();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}