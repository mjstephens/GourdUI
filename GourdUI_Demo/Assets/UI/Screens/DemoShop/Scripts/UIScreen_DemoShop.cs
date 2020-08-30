﻿using GourdUI;
using UnityEngine;
using UnityEngine.UI;

// public class UIScreen_DemoShop : UIScreen<IUIContract_DemoShop, UIState_DemoShop>
// {
//     #region Data
//
//     [Header("References")]
//     public DemoShopItemSelectionButtonInstance itemButtonPrefab;
//     public ShopCategoryData category1Data;
//     public ShopCategoryData category2Data;
//     public ShopCategoryData category3Data;
//
//     #endregion Data
//
//     
//     #region Setup
//     
//     protected override void SetupView()
//     {
//         // Setup listeners, events, hooks, etc. for new contract
//         _view.Category1SelectButton().onClick.AddListener(OnCategory1Selected);
//         _view.Category2SelectButton().onClick.AddListener(OnCategory2Selected);
//         _view.Category3SelectButton().onClick.AddListener(OnCategory3Selected);
//         _view.ExitShopButton().onClick.AddListener(OnExitShopButtonPressed);
//         _view.ItemDetailsPanel().purchaseItemButton.onClick.AddListener(
//             OnCurrentSelectedItemPurchased);
//         _view.ItemDetailsPanel().closePanelButton.onClick.AddListener(
//             OnCloseItemPanelButtonSelected);
//         
//         // Populate new items
//         PopulateItemGrid(category1Data);
//     }
//
//     protected override void TeardownView()
//     {
//         
//     }
//
//     #endregion Setup
//
//
//     #region State
//
//     protected override void CreateUIOverrideTypes()
//     {
//         _state = new UIState_DemoShop();
//         _view = new UIView_DemoShop();
//     }
//     
//     protected override void ResetScreenState()
//     { 
//         _state.currentSelectedCategory = 1;
//         _state.currentSelectedItem = null;
//     }
//     
//     protected override void ApplyScreenStateToCurrentView()
//     {
//         // Apply state to new view
//         switch (_state.currentSelectedCategory)
//         {
//             case 1:
//                 _state.currentSelectedCategory = 0;
//                 OnCategory1Selected(); 
//                 break;
//             case 2: 
//                 _state.currentSelectedCategory = 0;
//                 OnCategory2Selected();
//                 break;
//             case 3: 
//                 _state.currentSelectedCategory = 0;
//                 OnCategory3Selected(); 
//                 break;
//         }
//
//         if (_state.currentSelectedItem != null)
//         {
//             OnItemButtonSelected(_state.currentSelectedItem);
//         }
//         else
//         {
//             _view.ItemDetailsPanel().gameObject.SetActive(false);
//         }
//     }
//
//     #endregion State
//     
//
//     #region UI Logic
//
//     private void OnCategory1Selected()
//     {
//         if (_state.currentSelectedCategory != 1)
//         {
//             // Clear current items
//             _state.currentSelectedCategory = 1;
//             ClearItemGrid();
//             
//             // Populate new items
//             PopulateItemGrid(category1Data);
//         }
//     }
//     
//     private void OnCategory2Selected()
//     {
//         if (_state.currentSelectedCategory != 2)
//         {
//             // Clear current items
//             _state.currentSelectedCategory = 2;
//             ClearItemGrid();
//             
//             // Populate new items
//             PopulateItemGrid(category2Data);
//         }
//     }
//     
//     private void OnCategory3Selected()
//     {
//         if (_state.currentSelectedCategory != 3)
//         {
//             // Clear current items
//             _state.currentSelectedCategory = 3;
//             ClearItemGrid();
//             
//             // Populate new items
//             PopulateItemGrid(category3Data);
//         }
//     }
//
//     private void ClearItemGrid()
//     {
//         foreach (Transform child in _view.ItemGridParent())
//         {
//             Destroy(child.gameObject);
//         }
//     }
//
//     private void PopulateItemGrid(ShopCategoryData data)
//     {
//         foreach (ShopItemInstanceData item in data.categoryItems)
//         {
//             Transform grid = _view.ItemGridParent();
//             DemoShopItemSelectionButtonInstance button = Instantiate(itemButtonPrefab, grid);
//             button.PopulateSelection(item);
//             
//             // Add listener for item popup
//             button.GetComponent<Button>().onClick.AddListener(
//                 delegate{OnItemButtonSelected(item);});
//         }
//     }
//
//     private void OnItemButtonSelected(ShopItemInstanceData data)
//     {
//         _state.currentSelectedItem = data;
//         _view.ItemDetailsPanel().OnItemSelected(data);
//         _view.ItemDetailsPanel().gameObject.SetActive(true);
//     }
//
//     private void OnCloseItemPanelButtonSelected()
//     {
//         _state.currentSelectedItem = null;
//         _view.ItemDetailsPanel().gameObject.SetActive(false);
//     }
//
//     private void OnCurrentSelectedItemPurchased()
//     {
//         Debug.Log("Purchased: " + _state.currentSelectedItem.name);
//     }
//
//     private void OnExitShopButtonPressed()
//     {
//         GourdUI.GourdUI.Core.RemoveScreenFromStack(this);
//     }
//
//     #endregion  UI Logic
// }
