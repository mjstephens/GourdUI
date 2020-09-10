using GourdUI;
using UnityEngine;

public interface IUIContractView<in S> where S : UIState
{
    #region Properties

    /// <summary>
    /// The canvas for this view
    /// </summary>
    Canvas viewCanvas { get; }

    #endregion Properties
    
    
    /// <summary>
    /// Called directly after the view instance has been instantiated
    /// </summary>
    /// <param name="deviceData"></param>
    /// <param name="isScreenInstantiation"></param>
    void OnViewInstantiated(AppDeviceData deviceData, bool isScreenInstantiation);

    /// <summary>
    /// Called when the device data is updated.
    /// </summary>
    /// <param name="deviceData"></param>
    void OnAppDeviceDataUpdated(AppDeviceData deviceData);

    /// <summary>
    /// Should immediately apply screen state to view
    /// </summary>
    /// <param name="state"></param>
    /// <param name="isScreenInstantiation"></param>
    /// <param name="isFirstInstantiate"></param>
    void ApplyScreenStateToView(S state, bool isScreenInstantiation);
    
    /// <summary>
    /// Cleans up and destroys the view instance
    /// </summary>
    void OnDestroyView();
}