using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    public abstract class UIScreen<C,V,S>: MonoBehaviour, IUIScreen, IUIContractScreen
        where C : class, IUIContractScreen
        where V : IUIContractView
        where S : UIState, new()
    {
        #region Data

        [Header("Screen Data")]
        public UIScreenConfigDataTemplate configBaseData;

        protected V viewContract;
        protected S state;

        /// <summary>
        /// 
        /// </summary>
        private UIViewConfigData _currentViewData;
        
        #endregion Variables
        

        #region Initialization Methods

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
            
            // Apply reset state to view if applicable
            if (viewContract != null)
            {
                ApplyScreenStateToCurrentView();
            }
            
            // Activate view of needed
            if (configBaseData.data.activeOnLoad)
            {
                GourdUI.Core.AddScreenToStack(this, 0);
            }
            else
            {
                OnScreenDisabled();
            }
        }

        /// <summary>
        /// Called every time the screen instance is enabled.
        /// </summary>
        /// <param name="data"></param>
        public virtual void OnScreenEnabled<T>(T data = default)
        {
            gameObject.SetActive(true);
            ApplyScreenStateToCurrentView();
        }
        
        /// <summary>
        /// Called every time the screen instance is to be disabled.
        /// </summary>
        public virtual void OnScreenDisabled()
        {
            if (!configBaseData.data.preserveStateAfterScreenToggle)
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

        #endregion Initialization Methods


        #region View
        
        /// <summary>
        /// Responds to app device data updates.
        /// </summary>
        void IUIScreen.OnAppDeviceDataUpdated(AppDeviceData deviceData)
        {
            // Find valid UI views for the new device data
            FindValidUIView(deviceData);
            
            // Update view filter components on active view
            if (viewContract != null)
            {
                //_view.OnDeviceDataUpdate(deviceData);
            }
        }
        
        /// <summary>
        /// Sets the proper view for this screen based on AppDevice data.
        /// </summary>
        /// <param name="deviceData"></param>
        private void FindValidUIView(AppDeviceData deviceData)
        {
            // Find based on filters
            List<UIViewConfigData> _validViews  = new List<UIViewConfigData>();
            foreach (var view in configBaseData.data.views)
            {
                if (GourdUI.Core.UIViewIsValidForDevice(view.data, deviceData))
                {
                    _validViews.Add(view.data);
                }
            }
            
            // Do whatever with valid views
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
            
            //
            GameObject vObj = Instantiate(viewData.prefab, transform);
            viewContract = vObj.GetComponent<V>();
            vObj.GetComponent<UIView<C>>().screenContract = this as C;
            viewContract.OnViewPreSetup();
            
            // Setup view
            SetupView();
            
            //
            if (configBaseData.data.resetStateBetweenViewChanges)
            {
                ResetScreenState();
            }
            ApplyScreenStateToCurrentView();
        }

        /// <summary>
        /// Called when a new view for this screen is triggered for display.
        /// </summary>
        protected abstract void SetupView();

        #endregion View


        #region State
        
        protected abstract void ResetScreenState();

        protected abstract void ApplyScreenStateToCurrentView();

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
            //TODO
            
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
            return configBaseData.data;
        }

        #endregion Utility
    }
}