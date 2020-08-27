using GourdUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScreen_DemoShop : UIScreen
{
    #region View Interface

    public interface IDemoShopView : IUIView
    {
        TMP_Text CurrencyAmountText();
        DemoShopItemDetailsPanel ItemDetailsPanel();
        Transform ItemGridParent();
        UnityEngine.UI.Button Category1SelectButton();
        UnityEngine.UI.Button Category2SelectButton();
        UnityEngine.UI.Button Category3SelectButton();
    }
    private IDemoShopView _currentView;

    #endregion View Interface


    #region Data

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

    protected override void SetupView<T>(T viewAsInterface)
    {
        _currentView = viewAsInterface as IDemoShopView;
        _currentView.Category1SelectButton().onClick.AddListener(OnCategory1Selected);
        _currentView.Category2SelectButton().onClick.AddListener(OnCategory2Selected);
        _currentView.Category3SelectButton().onClick.AddListener(OnCategory3Selected);
        _currentView.ItemDetailsPanel().purchaseItemButton.onClick.AddListener(
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
        foreach (Transform child in _currentView.ItemGridParent())
        {
            Destroy(child.gameObject);
        }
    }

    private void PopulateItemGrid(ShopCategoryData data)
    {
        foreach (ShopItemInstanceData item in data.categoryItems)
        {
            Transform grid = _currentView.ItemGridParent();
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
        _currentView.ItemDetailsPanel().OnItemSelected(data);
        _currentView.ItemDetailsPanel().gameObject.SetActive(true);
    }

    private void OnCurrentSelectedItemPurchased()
    {
        Debug.Log("Purchased: " + _currentSelectedItem.name);
    }

    #endregion  UI Logic
}
