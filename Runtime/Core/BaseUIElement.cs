using UnityEngine;

namespace GourdUI
{
    public abstract class BaseUIElement : MonoBehaviour, IBaseUIElement
    {
        #region Variables

        private bool _hasSetup;
        // private bool _showFlag;
        // private bool _hideFlag;
        // private bool _cleanupFlag;
        //
        // private bool _manualDisableFlag;
        // private bool _manualEnableFlag;
        // private bool _manualDestroyFlag;

        #endregion Variables
        
        
        #region UI Commands

        /// <summary>
        /// Initial setup when the element is first instantiated.
        /// </summary>
        public virtual void Setup()
        {
            _hasSetup = true;
        }

        /// <summary>
        /// Final cleanup just before the element is destroyed.
        /// </summary>
        public virtual void Cleanup()
        {
            _hasSetup = false;
        }

        /// <summary>
        /// Called every time the element is activated.
        /// </summary>
        public virtual void Show()
        {
            // if (!CheckFlag(ref _showFlag))
            //     return;
            
            if (!_hasSetup)
            {
                Setup();
            }

            gameObject.SetActive(true);
        }

        /// <summary>
        /// Called everytime the element is deactivated.
        /// </summary>
        public virtual void Hide()
        {
            // if (!CheckFlag(ref _hideFlag))
            //     return;
            
            gameObject.SetActive(false);
        }

        #endregion UI Commands


        #region Lifecycle Events

        // private void OnEnable()
        // {
        //     if (!CheckFlag(ref _showFlag))
        //         return;
        //     
        //     if (!_hasSetup)
        //     {
        //         Setup();
        //     }
        //     
        //     Show();
        // }

        // private void OnDisable()
        // {
        //     if (!CheckFlag(ref _hideFlag))
        //         return;
        //     
        //     Hide();
        // }

        private void OnDestroy()
        {
            if (_hasSetup)
            {
                Cleanup();
            }
        }

        #endregion Lifecycle Events


        // #region Utility
        //
        // private static bool CheckFlag(ref bool flag)
        // {
        //     if (flag)
        //     {
        //         flag = false;
        //         return false;
        //     }
        //     flag = true;
        //     return true;
        // }
        //
        // #endregion Utility
    }
}