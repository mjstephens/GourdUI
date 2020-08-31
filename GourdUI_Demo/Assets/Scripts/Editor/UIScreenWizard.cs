using System;
using System.IO;
using System.Reflection;
using System.Text;
using GourdUI;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    /*
     *    Process works as follows:
     *     1) The directory tree for the new UI screen is created
     *     2) The data objects (UIScreenConfigDataTemplate and UIViewConfigDataTemplate) are created
     *     3) New classes are created from script templates, and are filled in with the correct class names
     *         based on the "screenName" passed in from the user
     *     4) The assets that have been created thus far are saved, and script compilation is triggered in
     *         order to access the new class types we just created
     *     5) After compilation, the final inter-connections are established (using the new types that were
     *         just compiled) and the system is finished.
     *
     *     Result:
     *     - A new directory structure under CONST_RootUIScreenPath
     *     - 5 new files (3 classes, 2 interfaces)
     *     - 2 new ScriptableObject assets (UIScreenConfigDataTemplate and UIViewConfigDataTemplate)
     *     - 2 new prefab assets (1 UIScreen stub prefab, 1 started UIView)
     */
    
    
    /// <summary>
    /// Automatically creates requisite scripts, data, and prefab objects for a new UIScreen.
    /// </summary>
    public static class UIScreenWizard
    {
        #region IDs

        // Directory
        public const string CONST_RootUIScreenPath = "Assets/UI/Screens/";

        // Class generation
        private const string CONST_ClassGenClassNameKey = "#CLASSNAME#";
        private const string CONST_ClassGenContractScreenKey = "#CONTRACTSCREEN#";
        private const string CONST_ClassGenContractViewKey = "#CONTRACTVIEW#";
        private const string CONST_ClassGenStateKey = "#STATE#";
        private const string CONST_ClassGenScreenKey = "#SCREEN#";
        private const string CONST_ScreenPrefabDataTemplateFieldName = "_configBaseData";
        
        // Editor pref keys
        private const string CONST_EditorPrefsScreenNameKey = "ScreenName";
        private const string CONST_EditorPrefsScreenClassGenType = "ScreenClassType";
        private const string CONST_EditorPrefsViewClassGenType = "ViewClassType";
        
        // Display
        private const string CONST_EditorProgressBarTitle = "Creating UIScreen Structure...";

        #endregion IDs


        #region Entry
        
        public static void OnCreateNewUIScreen(string screenName)
        {
            // We need to save the screen name since we need it later for the post-compilation steps
            EditorPrefs.SetString (CONST_EditorPrefsScreenNameKey, screenName);
            
            // Create project directory...
            Tuple<string,string,string,string> directoryPaths =
                CreateScreenDirectoryStructure(screenName);
            
            // Create project data items...
            Tuple<UIScreenConfigDataTemplate,UIViewConfigDataTemplate> dataObjects = 
                CreateScreenDataObjects(screenName);
            
            // Create scripts
            CreateScreenScripts(directoryPaths.Item3, screenName);
            
            // Finished!
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            CompilationPipeline.RequestScriptCompilation();
            
            // Select our new UIScreenConfigDataTemplate asset
            Selection.activeObject = dataObjects.Item1;
        }
        
        #endregion Entry
        

        #region Directory
        
        private static Tuple<string,string,string,string> 
            CreateScreenDirectoryStructure(string screenName)
        {
            // Progress bar
            EditorUtility.DisplayProgressBar(
                CONST_EditorProgressBarTitle, 
                "Directories", 0.2f);
            
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
        
        #endregion Directory

        
        #region ScriptableObject Data Assets
        
        private static Tuple<UIScreenConfigDataTemplate,UIViewConfigDataTemplate> 
            CreateScreenDataObjects(string screenName)
        {
            // Progress bar
            EditorUtility.DisplayProgressBar(
                CONST_EditorProgressBarTitle, 
                "Data Assets", 0.4f);
            
            string pathHeader = CONST_RootUIScreenPath + screenName + "/";

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
        
        #endregion ScriptableObject Data Assets
        

        #region Class Generation
        
        private static void CreateScreenScripts(string scriptsPath, string screenName)
        {
            // Progress bar
            EditorUtility.DisplayProgressBar(
                CONST_EditorProgressBarTitle, 
                "Compiling Scripts...", 0.6f);
            
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
            
            EditorPrefs.SetString (CONST_EditorPrefsScreenClassGenType, screenClass);
            EditorPrefs.SetString (CONST_EditorPrefsViewClassGenType, viewClass);
        }

        private static byte[] GetScriptTemplateContents(string className, string templatePath)
        {
            string contents = Resources.Load<TextAsset>(templatePath).text;
            contents = contents.Replace(CONST_ClassGenClassNameKey, className);
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
            contents = contents.Replace(CONST_ClassGenClassNameKey, className);
            contents = contents.Replace(CONST_ClassGenContractScreenKey, screenContractClass);
            contents = contents.Replace(CONST_ClassGenContractViewKey, viewContractClass);
            contents = contents.Replace(CONST_ClassGenStateKey, stateClass);

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
            contents = contents.Replace(CONST_ClassGenClassNameKey, className);
            contents = contents.Replace(CONST_ClassGenContractScreenKey, screenContractClass);
            contents = contents.Replace(CONST_ClassGenContractViewKey, viewContractClass);
            contents = contents.Replace(CONST_ClassGenScreenKey, screenClass);

            return new UTF8Encoding(true).GetBytes(contents);
        }
        
        #endregion Class Generation
        

        #region Recompilation

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptRecompile()
        {
            if (EditorPrefs.HasKey(CONST_EditorPrefsScreenClassGenType))
            {
                EditorUtility.DisplayProgressBar(
                    CONST_EditorProgressBarTitle, 
                    "Creating Prefabs", 0.8f);
                CreateScreenPrefabs();
                
                EditorPrefs.DeleteKey(CONST_EditorPrefsScreenNameKey);
                EditorPrefs.DeleteKey(CONST_EditorPrefsScreenClassGenType);
                EditorPrefs.DeleteKey(CONST_EditorPrefsViewClassGenType);
                
                EditorUtility.ClearProgressBar();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        #endregion Recompilation


        #region Prefab Objects

        private static void CreateScreenPrefabs()
        {
            string _screenName = EditorPrefs.GetString(CONST_EditorPrefsScreenNameKey);
            
            // Create screen object prefab
            GameObject screenPrefabObj = new GameObject {name = "UIScreen_" + _screenName};
            Type screenType = Type.GetType (EditorPrefs.GetString(CONST_EditorPrefsScreenClassGenType),
                true);
            screenPrefabObj.AddComponent(screenType);
            
            // Set data field
            UIScreenConfigDataTemplate screenData =
                (UIScreenConfigDataTemplate) AssetDatabase.LoadAssetAtPath(
                    CONST_RootUIScreenPath + _screenName + "/UIScreen_" + _screenName + "_Data.asset",
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
                        System.Reflection.FieldInfo[] fieldInfo = t.GetFields(
                            BindingFlags.Instance | BindingFlags.NonPublic);
                        foreach (System.Reflection.FieldInfo info in fieldInfo)
                        {
                            if (info.Name == CONST_ScreenPrefabDataTemplateFieldName)
                            {
                                info.SetValue(co, screenData);
                            }
                        }
                    }
                }
            }
            
            // Save prefab
            string path = CONST_RootUIScreenPath + _screenName + "/Prefabs/UIScreen_" + _screenName + ".prefab";
            PrefabUtility.SaveAsPrefabAssetAndConnect(
                screenPrefabObj, path, InteractionMode.UserAction);
            
            // Create view object prefab
            GameObject viewPrefabObj = new GameObject {name = "UIView_" + _screenName + "_Default"};
            Type viewType = Type.GetType (EditorPrefs.GetString(CONST_EditorPrefsViewClassGenType),
                true);
            Canvas c = viewPrefabObj.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler cs = viewPrefabObj.AddComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            viewPrefabObj.AddComponent<GraphicRaycaster>();
            viewPrefabObj.AddComponent(viewType);
            string viewPath = CONST_RootUIScreenPath + _screenName + "/Prefabs/UIView_" + _screenName + "_Default.prefab";
            GameObject viewPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(
                viewPrefabObj, viewPath, InteractionMode.UserAction);
            UnityEditor.Editor.DestroyImmediate(viewPrefabObj);
            
            // Assign view prefab to view data object
            UIViewConfigDataTemplate viewData =
                (UIViewConfigDataTemplate) AssetDatabase.LoadAssetAtPath(
                    CONST_RootUIScreenPath + _screenName + "/Views/UIView_" + _screenName + "_Default_Data.asset",
                    typeof(UIViewConfigDataTemplate));
            viewData.data.prefab = viewPrefab;
        }

        #endregion Prefab Objects
    }
}