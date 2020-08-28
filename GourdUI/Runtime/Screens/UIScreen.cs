using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    public abstract class UIScreen: MonoBehaviour
    {
        #region Data

        [Header("Screen Data")]
        public UIScreenConfigDataTemplate configBaseData;

        /// <summary>
        /// The current active UIView for this screen.
        /// </summary>
        private MonoUIView _currentViewObject;

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

        /// <summary>
        /// Called every time the screen instance is enabled.
        /// </summary>
        /// <param name="data"></param>
        public virtual void OnScreenEnabled<R>(R data = default)
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Called every time the screen instance is to be disabled.
        /// </summary>
        public virtual void OnScreenDisabled()
        {
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
        public void OnAppDeviceDataUpdated(AppDeviceData deviceData)
        {
            // Find valid UI views for the new device data
            FindValidUIView(deviceData);
            
            // Update view filter components on active view
            if (_currentViewObject != null)
            {
                (_currentViewObject as IUIViewContract).OnDeviceDataUpdate(deviceData);
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
        protected virtual void SetValidUIView(UIViewConfigData viewData)
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
            _currentViewObject = Instantiate(viewData.prefab, transform).GetComponent<MonoUIView>();
            
            // Setup view
            SetupView(_currentViewObject);
            
            // View should be g2g
            OnViewReady();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract"></param>
        /// <typeparam name="T"></typeparam>
        protected abstract void SetupView<T>(T contract) where T: IUIViewContract;

        /// <summary>
        /// 
        /// </summary>
        protected abstract void OnViewReady();

        #endregion View
        
        
        #region Implementation Methods

        /// <summary>
        /// Sets the render order of the UI canvas
        /// </summary>
        /// <param name="index"></param>
        public void OnScreenSetStackOrder(int index)
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

        #endregion Utility
    }
}