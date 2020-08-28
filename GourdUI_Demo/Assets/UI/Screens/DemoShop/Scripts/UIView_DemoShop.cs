using GourdUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIView_DemoShop : MonoUIView, IDemoShopViewContract
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

    Transform IDemoShopViewContract.ItemGridParent()
    {
        return _itemGridParent;
    }

    Button IDemoShopViewContract.Category1SelectButton()
    {
        return _category1SelectButton;
    }

    Button IDemoShopViewContract.Category2SelectButton()
    {
        return _category2SelectButton;
    }

    Button IDemoShopViewContract.Category3SelectButton()
    {
        return _category3SelectButton;
    }
}
