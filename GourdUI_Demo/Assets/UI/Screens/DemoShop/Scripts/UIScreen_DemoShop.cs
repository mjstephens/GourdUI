using GourdUI;
using UnityEngine;
using UnityEngine.UI;

public class UIScreen_DemoShop : UIScreen
{
    #region View Interface

    private IUIContract_DemoShop _viewContract;

    #endregion View Interface


    #region Data

    [Header("References")]
    public DemoShopItemSelectionButtonInstance itemButtonPrefab;
    public ShopCategoryData category1Data;
    public ShopCategoryData category2Data;
    public ShopCategoryData category3Data;

    #endregion Data


    #region State

    public class UIState_DemoShop
    {
        public int currentSelectedCategory;
        public ShopItemInstanceData currentSelectedItem;
    }

    private readonly UIState_DemoShop _state = new UIState_DemoShop
    {
        currentSelectedCategory = 1,
        currentSelectedItem = null
    };

    #endregion State
    
    
    #region Setup

    protected override void SetupView<T>(T contract)
    {
        _viewContract = contract as IUIContract_DemoShop;
        _viewContract.Category1SelectButton().onClick.AddListener(OnCategory1Selected);
        _viewContract.Category2SelectButton().onClick.AddListener(OnCategory2Selected);
        _viewContract.Category3SelectButton().onClick.AddListener(OnCategory3Selected);
        _viewContract.ItemDetailsPanel().purchaseItemButton.onClick.AddListener(
            OnCurrentSelectedItemPurchased);
        _viewContract.ItemDetailsPanel().closePanelButton.onClick.AddListener(
            OnCloseItemPanelButtonSelected);
        
        // Populate new items
        PopulateItemGrid(category1Data);
    }

    protected override void OnViewReady()
    {
        // Apply state
        switch (_state.currentSelectedCategory)
        {
            case 1:
                _state.currentSelectedCategory = 0;
                OnCategory1Selected(); 
                break;
            case 2: 
                _state.currentSelectedCategory = 0;
                OnCategory2Selected();
                break;
            case 3: 
                _state.currentSelectedCategory = 0;
                OnCategory3Selected(); 
                break;
        }

        if (_state.currentSelectedItem != null)
        {
            OnItemButtonSelected(_state.currentSelectedItem);
        }
        else
        {
            _viewContract.ItemDetailsPanel().gameObject.SetActive(false);
        }
    }

    #endregion Setup
    

    #region UI Logic

    private void OnCategory1Selected()
    {
        if (_state.currentSelectedCategory != 1)
        {
            // Clear current items
            _state.currentSelectedCategory = 1;
            ClearItemGrid();
            
            // Populate new items
            PopulateItemGrid(category1Data);
        }
    }
    
    private void OnCategory2Selected()
    {
        if (_state.currentSelectedCategory != 2)
        {
            // Clear current items
            _state.currentSelectedCategory = 2;
            ClearItemGrid();
            
            // Populate new items
            PopulateItemGrid(category2Data);
        }
    }
    
    private void OnCategory3Selected()
    {
        if (_state.currentSelectedCategory != 3)
        {
            // Clear current items
            _state.currentSelectedCategory = 3;
            ClearItemGrid();
            
            // Populate new items
            PopulateItemGrid(category3Data);
        }
    }

    private void ClearItemGrid()
    {
        foreach (Transform child in _viewContract.ItemGridParent())
        {
            Destroy(child.gameObject);
        }
    }

    private void PopulateItemGrid(ShopCategoryData data)
    {
        foreach (ShopItemInstanceData item in data.categoryItems)
        {
            Transform grid = _viewContract.ItemGridParent();
            DemoShopItemSelectionButtonInstance button = Instantiate(itemButtonPrefab, grid);
            button.PopulateSelection(item);
            
            // Add listener for item popup
            button.GetComponent<Button>().onClick.AddListener(
                delegate{OnItemButtonSelected(item);});
        }
    }

    private void OnItemButtonSelected(ShopItemInstanceData data)
    {
        _state.currentSelectedItem = data;
        _viewContract.ItemDetailsPanel().OnItemSelected(data);
        _viewContract.ItemDetailsPanel().gameObject.SetActive(true);
    }

    private void OnCloseItemPanelButtonSelected()
    {
        _state.currentSelectedItem = null;
    }

    private void OnCurrentSelectedItemPurchased()
    {
        Debug.Log("Purchased: " + _state.currentSelectedItem.name);
    }

    #endregion  UI Logic
}
