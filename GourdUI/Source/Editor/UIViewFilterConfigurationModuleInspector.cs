using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace GourdUI.Editor
{
    public class UIViewFilterConfigurationModuleInspector : UnityEditor.Editor
    {
        private ReorderableList posList;
        private ReorderableList negList;
        
        #region GUI

        private void OnEnable()
        {
            
        }

        public static void DrawViewFilters(ref UIViewFilterConfigurationModule filterData)
        {
            EditorGUILayout.LabelField("View Filters:", EditorStyles.boldLabel, GUILayout.Width(100));
            GUILayout.Space(2);
            DrawFilterList("Includes:", filterData.positiveFilters);
            DrawFilterList("Excludes:", filterData.negativeFilters);
        }
        
        private static void DrawFilterList(string label, List<UIViewFilterBaseConfigDataTemplate> targetList)
        {
            // Label
            EditorGUILayout.Space();
            Color defaultColor = GUI.backgroundColor;
            EditorGUILayout.LabelField(label);

            // Draw each list item
            int toRemove = -1;
            for (int i = 0; i < targetList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    toRemove = i;
                }
                GUI.backgroundColor = defaultColor;
                
                targetList[i] = (UIViewFilterBaseConfigDataTemplate)
                    EditorGUILayout.ObjectField(targetList[i], 
                        typeof(UIViewFilterBaseConfigDataTemplate), false);
                
                EditorGUILayout.EndHorizontal();
            }

            //
            if (toRemove != -1)
            {
                targetList.RemoveAt(toRemove);
            }
            
            // Add button
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.Width(80)))
            {
                targetList.Add(null);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        
        #endregion GUI
    }
}