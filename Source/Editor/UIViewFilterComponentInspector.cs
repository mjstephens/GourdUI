using UnityEditor;
using UnityEditorInternal;

namespace GourdUI.Editor
{
    // [CustomEditor(typeof(UIViewFilterComponent))]
    // public class UIViewFilterComponentInspector : UnityEditor.Editor
    // {
    //     #region Data
    //
    //     private UIViewFilterComponent _target;
    //     private UIViewFilterConfigurationModule _workingData;
    //     
    //     private ReorderableList posList;
    //     private ReorderableList negList;
    //
    //     #endregion Data
    //     
    //     
    //     #region Initialization
    //
    //     private void OnEnable()
    //     {
    //         posList = new ReorderableList(
    //             serializedObject, 
    //             serializedObject.FindProperty("filterData.positiveFilters"));
    //         negList = new ReorderableList(
    //             serializedObject, 
    //             serializedObject.FindProperty("filterData.negativeFilters"));
    //     } 
    //
    //     #endregion Initialization
    //     
    //     
    //     #region GUI
    //
    //     public override void OnInspectorGUI()
    //     {
    //         posList.DoLayoutList();
    //         negList.DoLayoutList();
    //         
    //         serializedObject.Update();
    //         serializedObject.ApplyModifiedProperties();
    //     }
    //     
    //     #endregion GUI
    // }
}