using System.Globalization;
using GourdUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIView_SampleHUD_Mobile : UIView<IUIContractScreen_SampleHUD, UIState_SampleHUD>, 
    IUIContractView_SampleHUD
{
    #region Fields

    [Header("UI References")] 
    public Image healthFill;
    public TMP_Text healthLabel;
    public TMP_Text coinsLabel;
    public UIHealthFlasher healthFlasher;

    #endregion Fields


    #region Setup

    public override void OnViewInstantiated()
    {
        // Optionally do things here after the view is instantiated
        
        // Use "screenContract" variable to access IUIContractScreen_SampleHUD
    }

    public override void ApplyScreenStateToView(UIState_SampleHUD state)
    {
        healthFill.fillAmount = state.healthValue / 100;
        healthLabel.text = state.healthValue.ToString(CultureInfo.InvariantCulture);
        coinsLabel.text = state.coinsValue.ToString(CultureInfo.InvariantCulture);
    }

    #endregion Setup
    
    
    #region Teardown

    protected override void OnViewPreDestroy()
    {
        // Optionally do things here before the view is destroyed
    }

    #endregion Teardown


    #region Screen Updates

    void IUIContractView_SampleHUD.OnReceiveNewHealthValue(float oldValue, float newValue)
    {
        healthFill.fillAmount = newValue / 100;
        healthLabel.text = newValue.ToString(CultureInfo.InvariantCulture);
        
        healthFlasher.Flash(oldValue > newValue);
    }

    void IUIContractView_SampleHUD.OnReceiveNewCoinsValue(int oldValue, int newValue)
    {
        coinsLabel.text = newValue.ToString(CultureInfo.InvariantCulture);
    }

    #endregion Screen Updates
}