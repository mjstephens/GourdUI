using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DemoShopItemDetailsPanel : MonoBehaviour
{
    #region References

    public Button purchaseItemButton;
    public Button closePanelButton;
    public TMP_Text itemNameText;
    public TMP_Text itemCostText;
    public Image itemIcon;

    #endregion References


    #region Setup

    public void OnItemSelected(ShopItemInstanceData data)
    {
        itemNameText.text = data.name;
        itemCostText.text = data.price.ToString();
        itemIcon.sprite = data.icon;
    }

    #endregion Setup
}