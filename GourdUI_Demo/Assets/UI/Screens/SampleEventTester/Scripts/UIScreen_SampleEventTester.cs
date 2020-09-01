using Game;
using GourdUI;
using UnityEngine;

/// <summary>
/// UIScreen implementation.
/// </summary>
public class UIScreen_SampleEventTester: 
    UIScreen <IUIContractScreen_SampleEventTester, IUIContractView_SampleEventTester, UIState_SampleEventTester>, 
    IUIContractScreen_SampleEventTester
{
    #region Setup

    protected override void SetupView()
    {
        // Use "viewContract" variable to access IUIContractView_SampleEventTester
    }

    #endregion Setup


    #region State

    protected override void ResetScreenState()
    {
        // Use "state" variable to access UIState_SampleEventTester
    }
    
    #endregion State


    #region Contract

    void IUIContractScreen_SampleEventTester.OnViewIncreaseHealth(float amount)
    {
        SampleGameController.Instance.OnCurrentHealthUpdated(amount);
    }

    void IUIContractScreen_SampleEventTester.OnViewDecreaseHealth(float amount)
    {
        SampleGameController.Instance.OnCurrentHealthUpdated(-amount);
    }

    void IUIContractScreen_SampleEventTester.OnViewIncreaseCoins(int amount)
    {
        SampleGameController.Instance.OnCurrentCoinsUpdated(amount);
    }

    void IUIContractScreen_SampleEventTester.OnViewDecreaseCoins(int amount)
    {
        SampleGameController.Instance.OnCurrentCoinsUpdated(-amount);
    }

    #endregion Contract
}