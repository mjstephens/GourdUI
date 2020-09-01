using UnityEngine;

namespace GourdUI
{
    public class UIViewFilterComponent : MonoBehaviour
    {
        #region Variables

        [Tooltip("Filters that must pass validation for this object to be active.")]
        public UIViewFilterBaseConfigDataTemplate[] componentFilters;

        #endregion Variables
    }
}