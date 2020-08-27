using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DemoShopItemSelectionButtonInstance : MonoBehaviour
{
    #region References

    public Image iconImage;

    #endregion References


    #region Initialization

    public void PopulateSelection(ShopItemInstanceData data)
    {
        iconImage.sprite = data.icon;
    }

    #endregion Initialization
}
