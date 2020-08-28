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

    private int _currentSelectedCategory = 1;
    private ShopItemInstanceData _currentSelectedItem;

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
        
        // Populate new items
        PopulateItemGrid(category1Data);
    }

    #endregion Setup
    

    #region UI Logic

    private void OnCategory1Selected()
    {
        if (_currentSelectedCategory != 1)
        {
            // Clear current items
            _currentSelectedCategory = 1;
            ClearItemGrid();
            
            // Populate new items
            PopulateItemGrid(category1Data);
        }
    }
    
    private void OnCategory2Selected()
    {
        if (_currentSelectedCategory != 2)
        {
            // Clear current items
            _currentSelectedCategory = 2;
            ClearItemGrid();
            
            // Populate new items
            PopulateItemGrid(category2Data);
        }
    }
    
    private void OnCategory3Selected()
    {
        if (_currentSelectedCategory != 3)
        {
            // Clear current items
            _currentSelectedCategory = 3;
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
        _currentSelectedItem = data;
        _viewContract.ItemDetailsPanel().OnItemSelected(data);
        _viewContract.ItemDetailsPanel().gameObject.SetActive(true);
    }

    private void OnCurrentSelectedItemPurchased()
    {
        Debug.Log("Purchased: " + _currentSelectedItem.name);
    }

    #endregion  UI Logic
}
