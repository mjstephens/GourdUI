using UnityEngine;

namespace GourdUI
{
    /// <summary>
    /// Base class for all UIViews. Holds reference to associated screen contract.
    /// See documentation for usage.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public abstract class UIView<C,S> : BaseUIElement, IUIView<S>
        where C : class, IUIContract
        where S : UIState
    {
        #region Properties

        /// <summary>
        /// The screen contract associated with this view
        /// </summary>
        public C screenContract { get; set; }
        
        public Canvas viewCanvas { get; private set;  }
        
        /// <summary>
        /// 
        /// </summary>
        private UIViewFilterComponent[] _filterComponents;

        protected S state { get; private set; }

        #endregion Properties
        
        
        #region View Methods
        
        public virtual void OnViewInstantiated(AppDeviceData deviceData, bool isScreenInstantiation)
        {
            // Gather view canvas
            viewCanvas = GetComponent<Canvas>();
            
            // Gather filter components in hierarchy
            _filterComponents = GetComponentsInChildren<UIViewFilterComponent>();
            FilterComponents(deviceData);
        }

        void IUIView<S>.OnAppDeviceDataUpdated(AppDeviceData deviceData)
        {
            FilterComponents(deviceData);
        }

        public void ApplyScreenStateToView(S s)
        {
            state = s;
        }
        
        void IUIView<S>.OnDestroyView()
        {
            Cleanup();
            Destroy(gameObject);
        }

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