using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GourdUI.Editor
{
    // [CustomPropertyDrawer(typeof(UIViewFilterConfigurationModule))]
    // public class UIViewFilterConfigurationModuleDrawer : PropertyDrawer
    // {
    //     private bool initialized;
    //     private ReorderableList posList;
    //     private ReorderableList negList;
    //     
    //     private void Initialize(SerializedProperty property) 
    //     {
    //         initialized = true;
    //         
    //         posList = new ReorderableList(
    //             property.serializedObject, 
    //             property.FindPropertyRelative("positiveFilters"));
    //         negList = new ReorderableList(
    //             property.serializedObject, 
    //             property.FindPropertyRelative("negativeFilters"));
    //     }
    //     
    //     
    //     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //     {
    //         if (!initialized)
    //             Initialize(property);
    //         
    //         EditorGUI.BeginProperty(position, label, property);
    //         EditorGUI.BeginChangeCheck();
    //
    //         posList.DoLayoutList();
    //         negList.DoLayoutList();
    //         
    //         if (EditorGUI.EndChangeCheck()) {
    //             property.serializedObject.ApplyModifiedProperties();
    //         }
    //         
    //         EditorGUI.EndProperty();
    //     }
    //     
    //     
    // }
}