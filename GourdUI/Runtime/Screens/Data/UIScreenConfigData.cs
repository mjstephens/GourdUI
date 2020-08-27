using UIGourd;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GourdUI
{
    [System.Serializable]
    public struct UIScreenConfigData
    {
        #region Enums
        
        public enum ScreenType
        {
            Active,
            Passive
        }

        #endregion Enums
        
        
        #region Variables

        [Header("Screen Options")] 
        public ScreenType screenType;
        public bool freezeSimulation;
        public bool lockUIUntilDismissed;
        public bool activeOnLoad;

        [Header("Views")] 
        public UIViewConfigDataTemplate[] views;

        [Header("Triggers")] 
        public UITriggerBaseConfigDataTemplate[] triggers;
        public string triggerCode;
        
        // #if ENABLE_INPUT_SYSTEM
        // public InputActionReference[] triggerActions;
        // #endif
        //
        // #if ENABLE_LEGACY_INPUT_MANAGER
        // public KeyCode[] triggerKeys;
        // #endif

        #endregion Variables
    }
}