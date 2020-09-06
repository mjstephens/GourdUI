using UIGourd;
using UnityEngine;
using UnityEngine.Rendering;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "New Screen Data", 
        menuName = "GourdUI/Screen Data")]
    public class UIScreenConfigData : ScriptableObject
    {
        #region Tooltips

        private const string CONST_orderGroupTooltip = "Canvases will be auto-ordered based on their position in the stack. You can optionally give screens a higher order 'group'. Higher groups will always render above lower groups, regardless of their position in the stack.";

        #endregion Tooltips
        
        
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
        [Tooltip(CONST_orderGroupTooltip)]
        public int screenOrderGroup;

        [Header("State Behavior")] 
        public bool resetStateBetweenViewChanges;
        public bool preserveStateAfterScreenToggle;

        [Header("Views")] 
        public UIViewConfigData[] views;

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