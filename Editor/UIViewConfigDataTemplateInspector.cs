using UnityEditor;
using UnityEngine;

namespace GourdUI.Editor
{
    // [CustomEditor(typeof(UIViewConfigDataTemplate))]
    // public class UIViewConfigDataTemplateInspector : UnityEditor.Editor
    // {
    //     #region Data
    //
    //     private UIViewConfigDataTemplate _target;
    //     private UIViewConfigData _workingData;
    //
    //     #endregion Data
    //     
    //     
    //     #region Initialization
    //
    //     private void OnEnable()
    //     {
    //         _target = (UIViewConfigDataTemplate)target;
    //         _workingData = _target.data;
    //         if (_workingData.filterData == null)
    //         {
    //             _workingData.filterData = new UIViewFilterConfigurationModule();
    //         }
    //     } 
    //
    //     #endregion Initialization
    //     
    //     
    //     #region GUI
    //
    //     public override void OnInspectorGUI()
    //     {
    //         DrawViewDataHeader();
    //         
    //         EditorGUILayout.Space(); 
    //         EditorUtilities.DrawGUILine(Color.gray);
    //         EditorGUILayout.Space();
    //         
    //         DrawViewPrefab(ref _workingData);
    //         
    //         base.OnInspectorGUI();
    //         return;
    //         ;
    //         EditorGUILayout.Space();
    //         EditorUtilities.DrawGUILine(Color.gray);
    //         EditorGUILayout.Space();
    //
    //         UIViewFilterConfigurationModuleInspector.DrawViewFilters(ref _workingData.filterData);
    //
    //         _target.data = _workingData;
    //         serializedObject.Update();
    //         serializedObject.ApplyModifiedProperties();
    //     } 
    //
    //     private void DrawViewDataHeader()
    //     {
    //         EditorGUILayout.Space();
    //         EditorGUILayout.BeginHorizontal();
    //         GUILayout.FlexibleSpace();
    //         EditorGUILayout.LabelField(_target.name, EditorStyles.boldLabel);
    //         GUILayout.FlexibleSpace();
    //         EditorGUILayout.EndHorizontal();
    //     }
    //
    //     private static void DrawViewPrefab(ref UIViewConfigData workingData)
    //     {
    //         EditorGUILayout.BeginHorizontal();
    //         EditorGUILayout.LabelField("View Prefab:", EditorStyles.boldLabel, GUILayout.Width(100));
    //         workingData.prefab = (GameObject)EditorGUILayout.ObjectField(
    //             workingData.prefab, 
    //             typeof(GameObject), 
    //             false);
    //         EditorGUILayout.EndHorizontal();
    //     }
    //
    //     #endregion GUI
    // }
}