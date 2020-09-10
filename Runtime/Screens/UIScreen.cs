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
        internal UIScreenConfigData _configBaseData;
        
        /// <summary>
        /// Cached view data 
        /// </summary>
        private UIViewConfigData _currentViewData;

        protected bool _screenHasBeenInstantiated;

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
                OnScreenDisabled();
            }
            
            // Persist screen if needed
            if (_configBaseData.persistent)
            {
                DontDestroyOnLoad(gameObject);
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
            if (viewContract != null && _screenHasBeenInstantiated)
            {
                viewContract.ApplyScreenStateToView(state, false);
            }
            
            _screenHasBeenInstantiated = true;
        }

        #endregion Activation

        
        #region Deactivation

        /// <summary>
        /// Called every time the screen instance is to be disabled.
        /// </summary>
        public virtual void OnScreenDisabled()
        {
            if (!_configBaseData.preserveStateAfterScreenToggle)
            {
                ResetScreenState();
            }
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            GourdUI.Core.RemoveScreenFromStack(this);
        }
        
        protected virtual void OnDestroy()
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
            if (viewContract != null)
            {
                if (_currentViewData.prefab != viewData.prefab)
                {
                    DestroyCurrentView();
                }
                else
                {
                    viewContract.OnAppDeviceDataUpdated(deviceData);
                    return;
                }
            }

            // Instantiate new view
            _currentViewData = viewData;
            
            // Instantiate the view prefab, hook up contract
            GameObject vObj = Instantiate(viewData.prefab, transform);
            viewContract = vObj.GetComponent<V>();
            vObj.GetComponent<UIView<C, S>>().screenContract = this as C;
            viewContract.OnViewInstantiated(deviceData, !_screenHasBeenInstantiated);

            // We can optionally reset the state between view changes
            if (_configBaseData.resetStateBetweenViewChanges)
            {
                ResetScreenState();
            }
            
            viewContract.ApplyScreenStateToView(state, !_screenHasBeenInstantiated);
        }

        #endregion View


        #region State
        
        /// <summary>
        /// Should reset the UI state to default values.
        /// </summary>
        protected abstract void ResetScreenState();
        
        protected virtual void OnViewPreDestroy() {}
        
        #endregion State
        
        
        #region Implementation Methods

        /// <summary>
        /// Sets the render order of the UI canvas
        /// </summary>
        /// <param name="index"></param>
        void IUIScreen.OnScreenSetStackOrder(int index)
        {
            // Set canvas of current active UIView
            viewContract.viewCanvas.sortingOrder = index;// + (_configBaseData.data.screenOrderGroup * 100);
        }
        
        #endregion Implementation Methods


        #region Utility

        private void DestroyCurrentView()
        {
            OnViewPreDestroy();
            viewContract?.OnDestroyView();
            viewContract = default;
        }
        
        UIScreenConfigData IUIScreen.ScreenConfigData()
        {
            return _configBaseData;
        }

        #endregion Utility
    }
}