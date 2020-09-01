using UnityEngine;

namespace GourdUI
{
    /// <summary>
    /// Base class for all UIViews. Holds reference to associated screen contract.
    /// See documentation for usage.
    /// </summary>
    public abstract class UIView<C,S> : MonoBehaviour, IUIContractView<S>
        where C : class, IUIContractScreen
        where S : UIState
    {
        #region Properties

        /// <summary>
        /// The screen contract associated with this view
        /// </summary>
        public C screenContract { get; set; }

        #endregion Properties
        
        
        #region View Methods

        public abstract void OnViewInstantiated();

        public abstract void ApplyScreenStateToView(S state);
        
        void IUIContractView<S>.OnDestroyView()
        {
            OnViewPreDestroy();
            Destroy(gameObject);
        }

        protected abstract void OnViewPreDestroy();

        #endregion View Methods
    }
}