using GourdUI;
using TMPro;
using UnityEngine;

public interface IUIContract_DemoShop : IUIContractScreen
{
    TMP_Text CurrencyAmountText();
    DemoShopItemDetailsPanel ItemDetailsPanel();
    Transform ItemGridParent();
    UnityEngine.UI.Button Category1SelectButton();
    UnityEngine.UI.Button Category2SelectButton();
    UnityEngine.UI.Button Category3SelectButton();
    UnityEngine.UI.Button ExitShopButton();
}
