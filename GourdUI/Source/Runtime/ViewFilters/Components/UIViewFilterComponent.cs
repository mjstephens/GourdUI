using UnityEngine;

namespace GourdUI
{
    /// <summary>
    /// Filter components can be attached to individual gameObjects in a UIView prefab to specficially
    /// enable that object on/off based on the filters provided. 
    /// </summary>
    public class UIViewFilterComponent : MonoBehaviour
    {
        #region Variables

        public UIViewFilterConfigurationModule filterData;

        #endregion Variables
    }
}