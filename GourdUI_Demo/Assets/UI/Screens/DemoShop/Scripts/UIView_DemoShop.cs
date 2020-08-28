using GourdUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIView_DemoShop : MonoUIView, IUIContract_DemoShop
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

    #endregion Variables


    public TMP_Text CurrencyAmountText()
    {
        return _currencyAmountText;
    }

    public DemoShopItemDetailsPanel ItemDetailsPanel()
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
}
