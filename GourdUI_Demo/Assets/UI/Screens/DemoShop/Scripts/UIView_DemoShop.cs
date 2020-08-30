using GourdUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIView_DemoShop : UIView<IUIContract_DemoShop>, IUIContract_DemoShop
{
    #region Variables

    [SerializeField]
    private TMP_Text _currencyAmountText;
    [SerializeField] 
    private DemoShopItemDetailsPanel _itemDetailsPanel;
    [SerializeField] 
    private Transform _itemGridParent;
    [SerializeField] 
    private Button _category1SelectButton;
    [SerializeField] 
    private Button _category2SelectButton;
    [SerializeField] 
    private Button _category3SelectButton;
    [SerializeField] 
    private Button _exitShopButton;

    #endregion Variables
    

    #region Contract

    TMP_Text IUIContract_DemoShop.CurrencyAmountText()
    {
        return _currencyAmountText;
    }

    DemoShopItemDetailsPanel IUIContract_DemoShop.ItemDetailsPanel()
    {
        return _itemDetailsPanel;
    }

    Transform IUIContract_DemoShop.ItemGridParent()
    {
        return _itemGridParent;
    }

    Button IUIContract_DemoShop.Category1SelectButton()
    {
        return _category1SelectButton;
    }

    Button IUIContract_DemoShop.Category2SelectButton()
    {
        return _category2SelectButton;
    }

    Button IUIContract_DemoShop.Category3SelectButton()
    {
        return _category3SelectButton;
    }
    
    Button IUIContract_DemoShop.ExitShopButton()
    {
        return _exitShopButton;
    }

    #endregion Contract


    #region State

    public override void OnViewPreSetup()
    {
        
    }

    // public override void OnStateDataUpdated<T>(T stateData)
    // {
    //     // Convert to specific state
    //     //UIScreen_DemoShop.UIState_DemoShop s = stateData as UIScreen_DemoShop.UIState_DemoShop;
    //     
    //     // Any components in the view that implement IUIStateUpdateReceivable will be notified
    //     // of the state change.
    //     
    // }

    #endregion State
}
