using GourdUI;
using UI.Screens.MainMenu.Scripts;

public class UIScreen_MainMenu : UIScreen
{
    #region View Interface

    private IUIContract_MainMenu _viewContract;

    #endregion View Interface
    
    
    #region State

    // public class UIState_MainMenu 
    // {
    //     public int currentSelectedCategory;
    //     public ShopItemInstanceData currentSelectedItem;
    // }
    //
    // private readonly UIState_MainMenu _state = new UIState_MainMenu
    // {
    //     currentSelectedCategory = 1,
    //     currentSelectedItem = null
    // };

    #endregion State
    
    
    #region Setup
    
    protected override void SetupView<T>(T contract)
    {
        _viewContract = contract as IUIContract_MainMenu;
        _viewContract.OpenDemoShopButton().onClick.AddListener(OnOpenDemoShopButtonSelected);
    }

    protected override void OnViewReady()
    {
        
    }
    
    #endregion Setup


    #region UI Logic

    private void OnOpenDemoShopButtonSelected()
    { 
        GourdUI.GourdUI.Core.ToggleUIScreen("demoShop");
    }

    #endregion UI Logic
}
