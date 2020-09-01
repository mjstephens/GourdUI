using UnityEngine;

namespace GourdUI
{
    /// <summary>
    /// Base class for all UIViews. Holds reference to associated screen contract.
    /// See documentation for usage.
    /// </summary>
    /// <typeparam name="C"></typeparam>
    public abstract class UIView<C> : MonoBehaviour, IUIContractView
        where C : class, IUIContractScreen
    {
        #region Properties

        /// <summary>
        /// The screen contract associated with this view
        /// </summary>
        public C screenContract { get; set; }

        #endregion Properties
        
        
        #region Initialization

        /// <summary>
        /// Called when the view has been selected
        /// </summary>
        public abstract void OnViewPreSetup();
        
        public void OnDestroyView()
        {
            Destroy(gameObject);
        }

        #endregion Initialization
    }
}