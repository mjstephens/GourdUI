using Game;
using GourdUI;
using UnityEngine;

/// <summary>
/// UIScreen implementation.
/// </summary>
public class UIScreen_SampleHUD: 
    UIScreen <IUIContractScreen_SampleHUD, IUIContractView_SampleHUD, UIState_SampleHUD>, 
    IUIContractScreen_SampleHUD
{
    #region Setup

    protected override void SetupView()
    {
        // Optionally implement setup for specific views
        
        // Use "viewContract" variable to access IUIContractView_SampleHUD
    }

    #endregion Setup
    
    
    #region State

    protected override void ResetScreenState()
    {
        // Use "state" variable to access UIState_SampleHUD
        state.healthValue = SampleGameController.Instance._currentHealth;
        state.coinsValue = SampleGameController.Instance._currentCoins;
    }

    #endregion State


    #region Game Events

    public void OnHealthUpdated(float oldValue, float newValue)
    {
        viewContract?.OnReceiveNewHealthValue(oldValue, newValue);
        state.healthValue = newValue;
    }
    
    public void OnCoinsUpdated(int oldValue, int newValue)
    {
        viewContract?.OnReceiveNewCoinsValue(oldValue, newValue);
        state.coinsValue = newValue;
    }

    #endregion Game Events
}