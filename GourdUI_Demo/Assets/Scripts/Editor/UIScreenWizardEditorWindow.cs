using System;
using Editor;
using UnityEngine;
using UnityEditor;

public class UIScreenWizardEditorWindow : EditorWindow
{
    #region Window

    [MenuItem("Window/GourdUI/UI Screen Wiard")]
    static void Init()
    {
        UIScreenWizardEditorWindow window = 
            (UIScreenWizardEditorWindow)GetWindow(typeof(UIScreenWizardEditorWindow));
        window.titleContent.text = "UIScreen Wizard";
        window.Show();
    }

    #endregion


    #region Variables

    private string _screenTitle;

    #endregion Variables
    
    
    #region Draw

    private void OnGUI()
    {
        // Option for entering name of new screen
        EditorGUILayout.Space(20);
        GUILayout.Label("Screen Name:");
        _screenTitle = EditorGUILayout.TextField(_screenTitle);
        GUILayout.Space(15);
        
        // Create button
        Color currentCol = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("CREATE"))
        {
            if (ValidateUIScreenName(_screenTitle))
            {
                UIScreenWizard.OnCreateNewUIScreen(_screenTitle);
            }
            else
            {
                Debug.LogWarning("Invalid UIScreen name!");
            }
        }
        GUI.backgroundColor = currentCol;
        
        // Display preview of package below
        DrawPackageContentsPreview();
    }

    private void DrawPackageContentsPreview()
    {
        GUILayout.Space(30);
        DrawGUILine(Color.gray);
        GUILayout.Space(10);

        GUILayout.Label("Contents preview:");
        GUILayout.Label("Assets/UI/Screens/" + _screenTitle);

        GUILayout.Space(15);
        Color currentCol = GUI.color;
        GUILayout.Label("[SCRIPTS]");
        GUI.color = Color.yellow;
        GUILayout.Label("UIScreen_" + _screenTitle + ".cs", EditorStyles.boldLabel);
        GUILayout.Label("UIView_" + _screenTitle + "_Default.cs", EditorStyles.boldLabel);
        GUILayout.Label("UIState_" + _screenTitle + ".cs", EditorStyles.boldLabel);
        GUILayout.Label("IUIContractView_" + _screenTitle + ".cs", EditorStyles.boldLabel);
        GUILayout.Label("IUIContractScreen_" + _screenTitle + ".cs", EditorStyles.boldLabel);
        
        GUILayout.Space(10);
        GUI.color = currentCol;
        GUILayout.Label("[DATA]");
        GUI.color = Color.green;
        GUILayout.Label("UIScreen_" + _screenTitle + "_Data", EditorStyles.boldLabel);
        GUILayout.Label("UIView_" + _screenTitle + "Default_Data", EditorStyles.boldLabel);
        
        GUILayout.Space(10);
        GUI.color = currentCol;
        GUILayout.Label("[PREFABS]");
        GUI.color = Color.cyan;
        GUILayout.Label("UIScreen_" + _screenTitle + ".prefab", EditorStyles.boldLabel);
        GUILayout.Label("UIView_" + _screenTitle + "_Default.prefab", EditorStyles.boldLabel);

        GUI.color = currentCol;
    }
    
    public static void DrawGUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
        r.height = thickness;
        r.y+=padding/2;
        r.x-=2;
        r.width +=6;
        EditorGUI.DrawRect(r, color);
    }

    #endregion Draw


    #region Utility

    private bool ValidateUIScreenName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;
        if (name == "UIScreen")
            return false;
        if (name == "CLASSNAME")
            return false;

        return true;
    }

    #endregion Utility
}
