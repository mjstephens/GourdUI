using UnityEngine;

namespace GourdUI
{
    // Base from:
    // https://connect.unity.com/p/updating-your-gui-for-the-iphone-x-and-other-notched-devices
    
    /// <summary>
    /// Updates the attached rect transform to anchor to the screen safe area
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour, ISafeArea
    {
        #region Variables

        /// <summary>
        /// The panel we are transforming.
        /// </summary>
        private RectTransform _panel;
        
        #endregion Variables

        
        #region Initialization

        private void Start ()
        {
            _panel = GetComponent<RectTransform> ();
            OnUISafeAreaUpdated(CoreDevice.UISafeArea());
        }

        private void OnEnable()
        {
            GourdUI.Device.RegisterSafeAreaComponent(this);
        }

        private void OnDisable()
        {
            GourdUI.Device.UnegisterSafeAreaComponent(this);
        }

        #endregion Initialization


        #region Safe Area

        public void OnUISafeAreaUpdated(Rect rect)
        {
            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            Vector2 anchorMin = rect.position;
            Vector2 anchorMax = rect.position + rect.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            _panel.anchorMin = anchorMin;
            _panel.anchorMax = anchorMax;
        }
        
        #endregion Safe Area
    }
}