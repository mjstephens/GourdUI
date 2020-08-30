using UnityEngine;

namespace GourdUI
{
    public abstract class UIView<C> : MonoBehaviour, IUIContractView
        where C : class, IUIContractScreen
    {
        public C screenContract;
        #region Initialization

    /// <summary>
    /// Called when the view has been selected
    /// </summary>
    public abstract void OnViewPreSetup();
    
    public void OnDestroyView()
    {
        Destroy(gameObject);
    }

    #endregion Initialization


    #region State

    //public abstract void OnStateDataUpdated<T>(T stateData) where T : UIState;

    #endregion State


    // #region Filter Components
    //

    // void IUIViewContract.OnDeviceDataUpdate(AppDeviceData deviceData)
    // {
    //    // FilterComponents(deviceData);
    // }
    //
    // /// <summary>
    // /// 
    // /// </summary>
    // private void FilterComponents(AppDeviceData deviceData)
    // {
    //     if (_filterComponents != null)
    //     {
    //         foreach (UIViewFilterComponent filterComponent in _filterComponents)
    //         {
    //             filterComponent.gameObject.SetActive(
    //                 UIViewFilterResolver.ViewFilterResult(
    //                     filterComponent.componentFilters,
    //                     deviceData));
    //         }
    //     }
    // }
    //
    // #endregion Filter Components
    }
}