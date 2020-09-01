using GourdUI;
using UnityEngine;
using UnityEngine.UI;

public class UIView_SampleEventTester_Default : UIView<IUIContractScreen_SampleEventTester, UIState_SampleEventTester>, 
    IUIContractView_SampleEventTester
{
    #region Fields
    
    

    #endregion Fields


    #region Setup

    public override void OnViewInstantiated()
    {
        // Optionally do things here after the view is instantiated
        
        // Use "screenContract" variable to access IUIContractScreen_SampleEventTester
    }

    public override void ApplyScreenStateToView(UIState_SampleEventTester state)
    {
        
    }

    #endregion Setup


    #region Teardown

    protected override void OnViewPreDestroy()
    {
        // Optionally do things here before the view is destroyed
    }

    #endregion Teardown


    #region UI Events

    public void OnHealthIncreaseButtonSelected()
    {
        screenContract.OnViewIncreaseHealth(5);
    }
    
    public void OnHealthDecreaseButtonSelected()
    {
        screenContract.OnViewDecreaseHealth(5);
    }
    
    public void OnCoinsIncreaseButtonSelected()
    {
        screenContract.OnViewIncreaseCoins(5);
    }
    
    public void OnCoinsDecreaseButtonSelected()
    {
        screenContract.OnViewDecreaseCoins(5);
    }

    #endregion UI Events


    #region Screen Updates

    

    #endregion Screen Updates
}