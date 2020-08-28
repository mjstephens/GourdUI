using GourdUI;
using TMPro;
using UnityEngine;


public interface IDemoShopViewContract : IUIViewContract
{
    TMP_Text CurrencyAmountText();
    DemoShopItemDetailsPanel ItemDetailsPanel();
    Transform ItemGridParent();
    UnityEngine.UI.Button Category1SelectButton();
    UnityEngine.UI.Button Category2SelectButton();
    UnityEngine.UI.Button Category3SelectButton();
}
