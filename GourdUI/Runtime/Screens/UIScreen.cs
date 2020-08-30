using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    public abstract class UIScreen<C,S>: MonoBehaviour, IUIScreen
        where C : class, IUIViewContract
        where S : UIState
    {
        #region Data

        [Header("Screen Data")]
        public UIScreenConfigDataTemplate configBaseData;

        protected C _currentContract;
        protected S _state;
        
        /// <summary>
        /// The current active UIView for this screen.
        /// </summary>
        private MonoUIView<C> _currentViewObject;

        /// <summary>
        /// 
        /// </summary>
        private UIViewConfigData _currentViewData;
        
        #endregion Variables
        

        #region Initialization Methods

        private void Start()
        {
            // Make sure the screen mono object is reset
            transform.position = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            
            // Register the screen
            GourdUI.Core.RegisterScreen(this);
        }

        /// <summary>
        /// Called once when the screen is first instantiated.
        /// </summary>
        public virtual void OnScreenInstantiated()
        {
            CreateUIState();
            
            FindValidUIView(GourdUI.Device.DeviceData());
            if (configBaseData.data.activeOnLoad)
            {
                GourdUI.Core.AddScreenToStack(this, 0);
            }
            else
            {
                OnScreenDisabled();
            }
        }

        protected abstract void CreateUIState();

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
            if (_currentViewObject != null)
            {
                _currentContract.OnDeviceDataUpdate(deviceData);
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
                if (_currentViewObject != null)
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
            if (_currentViewObject != null)
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
            _currentViewObject = Instantiate(viewData.prefab, transform).GetComponent<MonoUIView<C>>();
            _currentContract = _currentViewObject as C;
            
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
        /// <param name="contract"></param>
        /// <typeparam name="T"></typeparam>
        protected abstract void SetupView();

        #endregion View


        #region State
        
        protected abstract void ResetScreenState();

        protected virtual void ApplyScreenStateToCurrentView()
        {
            //_currentViewObject.OnStateDataUpdated();
        }

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
            Destroy(_currentViewObject.gameObject);
        }
        
        UIScreenConfigData IUIScreen.ScreenConfigData()
        {
            return configBaseData.data;
        }

        #endregion Utility
    }
}