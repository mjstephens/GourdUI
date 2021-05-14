using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

namespace GourdUI.Editor
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
        public const string CONST_ClassTemplatesDirectory = "Packages/com.mstephens.gourd_ui/Data/Templates/";

        // Class generation
        private const string CONST_ClassGenClassNameKey = "#CLASSNAME#";
        private const string CONST_ClassGenContractScreenKey = "#CONTRACTSCREEN#";
        private const string CONST_ClassGenStateKey = "#STATE#";
        private const string CONST_ClassGenScreenKey = "#SCREEN#";
        private const string CONST_ClassGenViewKey = "#VIEW#";
        private const string CONST_ScreenPrefabDataTemplateFieldName = "_configBaseData";
        
        // Editor pref keys
        private const string CONST_EditorPrefsScreenNameKey = "ScreenName";
        private const string CONST_EditorPrefsScreenClassGenType = "ScreenClassType";
        private const string CONST_EditorPrefsViewClassGenType = "ViewClassType";
        
        // Display
        private const string CONST_EditorProgressBarTitle = "Creating UIScreen Structure...";

        #endregion IDs


        #region Entry
        
        // Receives command to begin construction from the editor window (UIScreenWizardEditorWindow)
        public static void OnCreateNewUIScreen(string screenName)
        {
            // We need to save the screen name since we need it later for the post-compilation steps
            EditorPrefs.SetString (CONST_EditorPrefsScreenNameKey, screenName);
            
            // Create project directory...
            Tuple<string,string,string,string> directoryPaths =
                CreateScreenDirectoryStructure(screenName);
            
            // Create project data items...
            Tuple<UIScreenConfigData,UIViewConfigData> dataObjects = 
                CreateScreenDataObjects(screenName);
            
            // Create scripts
            CreateScreenScripts(directoryPaths.Item3, screenName);
            
            // Recompile scripts to access generated classes (step 4 above)
            // OnScriptRecompile() will be called automatically
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
            
            return new Tuple<string, string, string, string>(
                rootScreenPath.FullName,
                screenPrefabsPath.FullName,
                screenScriptsPath.FullName,
                screenViewsPath.FullName);
        }
        
        #endregion Directory

        
        #region ScriptableObject Data Assets
        
        private static Tuple<UIScreenConfigData,UIViewConfigData> 
            CreateScreenDataObjects(string screenName)
        {
            // Progress bar
            EditorUtility.DisplayProgressBar(
                CONST_EditorProgressBarTitle, 
                "Data Assets", 0.4f);
            
            string pathHeader = CONST_RootUIScreenPath + screenName + "/";

            // Create view object data
            UIViewConfigData viewDataObj =
                ScriptableObject.CreateInstance<UIViewConfigData>();
            AssetDatabase.CreateAsset(
                viewDataObj, 
                pathHeader + "Views/UIView_" + screenName + "_Default_Data.asset");
            
            // Create screen object data
            UIScreenConfigData screenDataObj =
                ScriptableObject.CreateInstance<UIScreenConfigData>();
            
            // Assign view to screen
            screenDataObj.views = new[] {viewDataObj};
            screenDataObj.triggerCode = screenName.ToLower();
            AssetDatabase.CreateAsset(
                screenDataObj, 
                pathHeader + "UIScreen_" + screenName + "_Data.asset");
            
            // Save the assets
            return new Tuple<UIScreenConfigData, UIViewConfigData>(screenDataObj, viewDataObj);
        }
        
        #endregion ScriptableObject Data Assets
        

        #region Class Generation
        
        private static void CreateScreenScripts(string scriptsPath, string screenName)
        {
            // Progress bar
            EditorUtility.DisplayProgressBar(
                CONST_EditorProgressBarTitle, 
                "Compiling Scripts...", 0.6f);
            
            // Class names
            string contractScreenClass = "IUIContract_" + screenName;
            string stateClass = "UIState_" + screenName;
            string screenClass = "UIScreen_" + screenName;
            string viewClass = "UIView_" + screenName;

            // Screen contract
            using (FileStream fs = File.Create(Path.Combine(scriptsPath, contractScreenClass + ".cs")))
            {
                byte[] data = GetScriptTemplateContents(
                    contractScreenClass, 
                    "IUIContractScreenTemplate.txt",
                    contractScreenClass,
                    stateClass,
                    screenClass,
                    viewClass);
                fs.Write(data, 0, data.Length);
            }

            // UI state
            using (FileStream fs = File.Create(Path.Combine(scriptsPath, stateClass + ".cs")))
            {
                byte[] data = GetScriptTemplateContents(
                    stateClass, 
                    "UIStateTemplate.txt",
                    contractScreenClass,
                    stateClass,
                    screenClass,
                    viewClass);
                fs.Write(data, 0, data.Length);
            } 
            
            // UI screen
            using (FileStream fs = File.Create(Path.Combine(scriptsPath, screenClass + ".cs")))
            {
                byte[] data = GetScriptTemplateContents(
                    screenClass, 
                    "UIScreenTemplate.txt",
                    contractScreenClass,
                    stateClass,
                    screenClass,
                    viewClass);
                fs.Write(data, 0, data.Length);
            } 

            // UI view
            using (FileStream fs = File.Create(Path.Combine(scriptsPath, viewClass + ".cs")))
            {
                byte[] data = GetScriptTemplateContents(
                    viewClass, 
                    "UIViewTemplate.txt",
                    contractScreenClass,
                    stateClass,
                    screenClass,
                    viewClass);
                fs.Write(data, 0, data.Length);
            } 
            
            EditorPrefs.SetString (CONST_EditorPrefsScreenClassGenType, screenClass);
            EditorPrefs.SetString (CONST_EditorPrefsViewClassGenType, viewClass);
        }
        
        private static byte[] GetScriptTemplateContents(
            string className, 
            string templatePath,
            string screenContractClass,
            string stateClass,
            string screenClass,
            string viewClass)
        {
            string contents = AssetDatabase.LoadAssetAtPath<TextAsset>(
                CONST_ClassTemplatesDirectory + templatePath).text;
            contents = contents.Replace(CONST_ClassGenClassNameKey, className);
            contents = contents.Replace(CONST_ClassGenContractScreenKey, screenContractClass);
            contents = contents.Replace(CONST_ClassGenStateKey, stateClass);
            contents = contents.Replace(CONST_ClassGenScreenKey, screenClass);
            contents = contents.Replace(CONST_ClassGenViewKey, viewClass);

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

                OnWizardFinished();
            }
        }

        #endregion Recompilation


        #region Prefab Objects

        private static void CreateScreenPrefabs()
        {
            string _screenName = EditorPrefs.GetString(CONST_EditorPrefsScreenNameKey);
            
            // Create screen object prefab
            GameObject screenPrefabObj = new GameObject {name = "UIScreen_" + _screenName};
            Type screenType = System.AppDomain.CurrentDomain.GetTypeByName(
                    EditorPrefs.GetString(CONST_EditorPrefsScreenClassGenType));
            screenPrefabObj.AddComponent(screenType);
            
            // Set data field
            UIScreenConfigData screenData =
                (UIScreenConfigData) AssetDatabase.LoadAssetAtPath(
                    CONST_RootUIScreenPath + _screenName + "/UIScreen_" + _screenName + "_Data.asset",
                    typeof(UIScreenConfigData));
            ReflectionHelpers.SetTypeFieldValue(
                screenPrefabObj, screenType, CONST_ScreenPrefabDataTemplateFieldName, screenData);
            
            // Save prefab
            string path = CONST_RootUIScreenPath + _screenName + "/Prefabs/UIScreen_" + _screenName + ".prefab";
            PrefabUtility.SaveAsPrefabAssetAndConnect(
                screenPrefabObj, path, InteractionMode.UserAction);
            
            // Create view object prefab
            GameObject viewPrefabObj = new GameObject {name = "UIView_" + _screenName + "_Default"};
            Type viewType = System.AppDomain.CurrentDomain.GetTypeByName(
                EditorPrefs.GetString(CONST_EditorPrefsViewClassGenType));
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
            UIViewConfigData viewData =
                (UIViewConfigData) AssetDatabase.LoadAssetAtPath(
                    CONST_RootUIScreenPath + _screenName + "/Views/UIView_" + _screenName + "_Default_Data.asset",
                    typeof(UIViewConfigData));
            viewData.prefab = viewPrefab;
        }

        #endregion Prefab Objects


        #region Finish

        /// <summary>
        /// Wraps up wizard stuff
        /// </summary>
        private static void OnWizardFinished()
        {
            EditorPrefs.DeleteKey(CONST_EditorPrefsScreenNameKey);
            EditorPrefs.DeleteKey(CONST_EditorPrefsScreenClassGenType);
            EditorPrefs.DeleteKey(CONST_EditorPrefsViewClassGenType);
                
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }

        #endregion Finish
    }
}