using System;
using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    /// <summary>
    /// Base class for all UIScreens. Holds reference to contracts and current view data.
    /// See documentation for usage.
    /// </summary>
    public abstract class UIScreen<C,V,S>: BaseUIElement, IUIScreen, IUIContract
        where C : class, IUIContract
        where V : IUIView<S>
        where S : UIState, new()
    {
        #region Fields

        [Header("Screen Data")]
        [SerializeField]
        // We declare this as internal to enable reflection access in the UIScreen Wizard
        internal UIScreenConfigData _configBaseData;
        
        /// <summary>
        /// Cached view data 
        /// </summary>
        private UIViewConfigData _currentViewData;

        private bool _screenHasBeenInstantiated;

        #endregion Fields


        #region Properties

        /// <summary>
        /// The active IUIContractView for this screen.
        /// </summary>
        private V _viewContract;
        
        /// <summary>
        /// The active UIState for this screen.
        /// </summary>
        protected S state;

        #endregion Properties
        

        #region Setup

        private void Start()
        {
            Setup();
        }

        // Implementing setup/cleanup instead of start
        public override void Setup()
        {
            base.Setup();
            
            // Register screen
            GourdUI.Core.RegisterScreen(this);
            
            // Create state class
            state = new S();
            
            // Make sure our state is reset for first initialization
            ResetScreenState();

            // Find the correct UI view
            FindValidUIView(GourdUI.Device.DeviceData());

            // Activate view if it should be active by default
            if (_configBaseData.activeOnLoad)
            {
                GourdUI.Core.AddScreenToStack(this, 0);
            }
            else
            {
                Hide();
            }
            
            // Persist screen if needed
            if (_configBaseData.persistent)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        
        /// <summary>
        /// Called every time the screen instance is enabled.
        /// </summary>
        public override void Show()
        {
            base.Show();
            
            // Make sure we update the view to match most recent state config.
            if (_viewContract != null && _screenHasBeenInstantiated)
            {
                _viewContract.ApplyScreenStateToView(state);
            }
            
            _screenHasBeenInstantiated = true;
        }

        #endregion Setup

        
        #region Teardown

        public override void Hide()
        {
            base.Hide();
            
            if (!_configBaseData.preserveStateAfterScreenToggle)
            {
                ResetScreenState();
            }
            GourdUI.Core.RemoveScreenFromStack(this);
        }

        public override void Cleanup()
        {
            base.Cleanup();
            
            GourdUI.Core.UnregisterScreen(this);
        }

        #endregion Teardown


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
            foreach (var view in _configBaseData.views)
            {
                if (GourdUI.Core.UIViewIsValidForDevice(view, deviceData))
                {
                    _validViews.Add(view);
                }
            }

            // TODO: implement some sort of priority/points system to resolve multiple passing views
            if (_validViews.Count > 0)
            {
                SetValidUIView(deviceData, _validViews[0]);
            }
            else
            {
                DestroyCurrentView();
            }
        }
        
        /// <summary>
        /// Sets the valid UIView as active once it's been found.
        /// </summary>
        /// <param name="deviceData"></param>
        /// <param name="viewData"></param>
        private void SetValidUIView(AppDeviceData deviceData, UIViewConfigData viewData)
        {
            // Make sure we're not setting the same UI view
            if (_viewContract != null)
            {
                if (_currentViewData.prefab != viewData.prefab)
                {
                    DestroyCurrentView();
                }
                else
                {
                    _viewContract.OnAppDeviceDataUpdated(deviceData);
                    return;
                }
            }

            // Instantiate new view
            _currentViewData = viewData;
            
            // Instantiate the view prefab, hook up contract
            GameObject vObj = Instantiate(viewData.prefab, transform);
            _viewContract = vObj.GetComponent<V>();
            vObj.GetComponent<UIView<C, S>>().screenContract = this as C;
            _viewContract.OnViewInstantiated(deviceData, !_screenHasBeenInstantiated);

            // We can optionally reset the state between view changes
            if (_configBaseData.resetStateBetweenViewChanges)
            {
                ResetScreenState();
            }
            
            _viewContract.ApplyScreenStateToView(state);
            _viewContract.Show();
        }

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
            // Set canvas of current active UIView
            if (!_configBaseData.ignoreAutoStacking)
            {
                _viewContract.viewCanvas.sortingOrder = index;
            }
        }
        
        #endregion Implementation Methods


        #region Utility

        private void DestroyCurrentView()
        {
            _viewContract?.OnDestroyView();
            _viewContract = default;
        }
        
        UIScreenConfigData IUIScreen.ScreenConfigData()
        {
            return _configBaseData;
        }

        #endregion Utility
    }
}