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

        private UIViewFilterComponent[] _filterComponents;

        #endregion Properties
        
        
        #region View Methods

        public virtual void OnViewInstantiated(AppDeviceData deviceData)
        {
            // Gather filter components in hierarchy
            _filterComponents = GetComponentsInChildren<UIViewFilterComponent>();
            FilterComponents(deviceData);
        }

        void IUIContractView<S>.OnAppDeviceDataUpdated(AppDeviceData deviceData)
        {
            FilterComponents(deviceData);
        }

        public abstract void ApplyScreenStateToView(S state);
        
        void IUIContractView<S>.OnDestroyView()
        {
            OnViewPreDestroy();
            Destroy(gameObject);
        }

        /// <summary>
        /// Called directly before the view instance is destroyed.
        /// </summary>
        protected abstract void OnViewPreDestroy();

        #endregion View Methods


        #region Filter Components

        /// <summary>
        /// Cycles through filter components in this view and evaluates
        /// </summary>
        /// <param name="deviceData"></param>
        private void FilterComponents(AppDeviceData deviceData)
        {
            foreach (var c in _filterComponents)
            {
                c.gameObject.SetActive(
                    UIViewFilterResolver.ViewFilterResult(
                        c.filterData.positiveFilters.ToArray(),
                        c.filterData.negativeFilters.ToArray(),
                        deviceData));
            }
        }

        #endregion Filter Components
    }
}