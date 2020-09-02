using UnityEditor;

namespace GourdUI.Editor
{
    [CustomEditor(typeof(UIViewFilterComponent))]
    public class UIViewFilterComponentInspector : UnityEditor.Editor
    {
        #region Data

        private UIViewFilterComponent _target;
        private UIViewFilterConfigurationModule _workingData;

        #endregion Data
        
        
        #region Initialization

        private void OnEnable()
        {
            _target = (UIViewFilterComponent)target;
            _workingData = _target.filterData;
            if (_workingData == null)
            {
                _workingData = new UIViewFilterConfigurationModule();
            }
        } 

        #endregion Initialization
        
        
        #region GUI

        public override void OnInspectorGUI()
        {
            UIViewFilterConfigurationModuleInspector.DrawViewFilters(ref _workingData);

            _target.filterData = _workingData;
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
        
        #endregion GUI
    }
}