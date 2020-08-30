using GourdUI;
using UI.Screens.MainMenu.Scripts;

public class UIScreen_MainMenu : UIScreen <IUIContract_MainMenu, UIState_MainMenu>
{
    #region View Interface

    private IUIContract_MainMenu _viewContract;

    #endregion View Interface
    
    
    #region Setup
    
    protected override void ApplyScreenStateToCurrentView()
    {
        
    }

    

    protected override void SetupView()
    {
        _currentContract.OpenDemoShopButton().onClick.AddListener(OnOpenDemoShopButtonSelected);
    }

    
    
    #endregion Setup


    #region State
    
    protected override void CreateUIState()
    {
        _state = new UIState_MainMenu();
    }

    protected override void ResetScreenState()
    {
        
    }

    #endregion State


    #region UI Logic

    private void OnOpenDemoShopButtonSelected()
    { 
        GourdUI.GourdUI.Core.ToggleUIScreen("demoShop");
    }

    #endregion UI Logic
}
