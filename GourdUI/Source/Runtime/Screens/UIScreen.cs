using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    /// <summary>
    /// Base class for all UIScreens. Holds reference to contracts and current view data.
    /// See documentation for usage.
    /// </summary>
    public abstract class UIScreen<C,V,S>: MonoBehaviour, IUIScreen, IUIContractScreen
        where C : class, IUIContractScreen
        where V : IUIContractView<S>
        where S : UIState, new()
    {
        #region Fields

        [Header("Screen Data")]
        [SerializeField]
        // We declare this as internal to enable reflection access in the UIScreen Wizard
        internal UIScreenConfigDataTemplate _configBaseData;

        /// <summary>
        /// Cached view data 
        /// </summary>
        private UIViewConfigData _currentViewData;
        
        #endregion Fields


        #region Properties

        /// <summary>
        /// The active IUIContractView for this screen.
        /// </summary>
        protected V viewContract;
        
        /// <summary>
        /// The active UIState for this screen.
        /// </summary>
        protected S state;

        #endregion Properties
        

        #region Initialization

        private void Start()
        {
            // Register the screen
            GourdUI.Core.RegisterScreen(this);
        }

        /// <summary>
        /// Called once when the screen is first instantiated.
        /// </summary>
        public virtual void OnScreenInstantiated()
        {
            // Create state class
            state = new S();

            // Find the correct UI view
            FindValidUIView(GourdUI.Device.DeviceData());
            
            // Make sure our state is reset for first initialization
            ResetScreenState();
            
            // Apply reset state to view
            if (viewContract != null)
            {
                viewContract.ApplyScreenStateToView(state);
            }
            
            // Activate view if it should be active by default
            if (_configBaseData.data.activeOnLoad)
            {
                GourdUI.Core.AddScreenToStack(this, 0);
            }
            else
            {
                OnScreenDisabled();
            }
        }

        #endregion Initialization


        #region Activation

        /// <summary>
        /// Called every time the screen instance is enabled.
        /// </summary>
        /// <param name="data"></param>
        public virtual void OnScreenEnabled<T>(T data = default)
        {
            gameObject.SetActive(true);
            
            // Make sure we update the view to match most recent state config.
            viewContract.ApplyScreenStateToView(state);
        }

        #endregion Activation

        
        #region Deactivation

        /// <summary>
        /// Called every time the screen instance is to be disabled.
        /// </summary>
        public virtual void OnScreenDisabled()
        {
            if (!_configBaseData.data.preserveStateAfterScreenToggle)
            {
                ResetScreenState();
            }
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            GourdUI.Core.RemoveScreenFromStack(this);
        }
        
        private void OnDestroy()
        {
            GourdUI.Core.UnregisterScreen(this);
        }

        #endregion Deactivation


        #region View
        
        void IUIScreen.OnAppDeviceDataUpdated(AppDeviceData deviceData)
        {
            FindValidUIView(deviceData);
        }
        
        /// <summary>
        /// Sets the proper view for this screen based on AppDevice data.
        /// </summary>
        /// <param name="deviceData"></param>
        private void FindValidUIView(AppDeviceData deviceData)
        {
            // Find based on filters
            List<UIViewConfigData> _validViews  = new List<UIViewConfigData>();
            foreach (var view in _configBaseData.data.views)
            {
                if (GourdUI.Core.UIViewIsValidForDevice(view.data, deviceData))
                {
                    _validViews.Add(view.data);
                }
            }

            // TODO: implement some sort of priority/points system to resolve multiple passing views
            if (_validViews.Count > 0)
            {
                SetValidUIView(_validViews[0]);
            }
            else
            {
                if (viewContract != null)
                {
                    DestroyCurrentView();
                }
            }
        }
        
        /// <summary>
        /// Sets the valid UIView as active once it's been found.
        /// </summary>
        /// <param name="viewData"></param>
        private void SetValidUIView(UIViewConfigData viewData)
        {
            // Make sure we're not setting the same UI view
            if (viewContract != null)
            {
                if (_currentViewData.prefab != viewData.prefab)
                {
                    DestroyCurrentView();
                }
                else
                {
                    return;
                }
            }

            // Instantiate new view
            _currentViewData = viewData;
            
            // Instantiate the view prefab, hook up contract
            GameObject vObj = Instantiate(viewData.prefab, transform);
            viewContract = vObj.GetComponent<V>();
            vObj.GetComponent<UIView<C, S>>().screenContract = this as C;
            viewContract.OnViewInstantiated();
            
            // Setup view
            SetupView();
            
            // We can optionally reset the state between view changes
            if (_configBaseData.data.resetStateBetweenViewChanges)
            {
                ResetScreenState();
            }
            
            viewContract.ApplyScreenStateToView(state);
        }

        /// <summary>
        /// Called when a new view for this screen is triggered for display.
        /// </summary>
        protected abstract void SetupView();

        #endregion View


        #region State
        
        /// <summary>
        /// Should reset the UI state to default values.
        /// </summary>
        protected abstract void ResetScreenState();

        #endregion State
        
        
        #region Implementation Methods

        /// <summary>
        /// Sets the render order of the UI canvas
        /// </summary>
        /// <param name="index"></param>
        void IUIScreen.OnScreenSetStackOrder(int index)
        {
            // if (controlCanvas != null)
            // {
            //     controlCanvas.sortingOrder = index + configBaseData.data.canvasRenderOrderAddition;
            // }
            //TODO fix this
            
            // Set canvas of current active UIView
        }
        
        #endregion Implementation Methods


        #region Utility

        private void DestroyCurrentView()
        {
            viewContract.OnDestroyView();
        }
        
        UIScreenConfigData IUIScreen.ScreenConfigData()
        {
            return _configBaseData.data;
        }

        #endregion Utility
    }
}