using UnityEngine;

namespace GourdUI
{
    public abstract class MonoUIView<C> : MonoBehaviour, IUIViewContract
        where C : IUIViewContract
    {
        #region Variables

        private UIViewFilterComponent[] _filterComponents;

        #endregion Variables
        
        
        #region Initialization

        protected virtual void Start()
        {
            _filterComponents = GetComponentsInChildren<UIViewFilterComponent>();
            FilterComponents(GourdUI.Device.DeviceData());
        }

        #endregion Initialization


        #region State

        public abstract void OnStateDataUpdated<T>(T stateData) where T : UIState;

        #endregion State


        #region Filter Components
        
        void IUIViewContract.OnDeviceDataUpdate(AppDeviceData deviceData)
        {
            FilterComponents(deviceData);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void FilterComponents(AppDeviceData deviceData)
        {
            if (_filterComponents != null)
            {
                foreach (UIViewFilterComponent filterComponent in _filterComponents)
                {
                    filterComponent.gameObject.SetActive(
                        UIViewFilterResolver.ViewFilterResult(
                            filterComponent.componentFilters,
                            deviceData));
                }
            }
        }

        #endregion Filter Components
    }
}