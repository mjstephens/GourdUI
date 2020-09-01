using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthFlasher : MonoBehaviour
{
    #region References

    public Image flashImage;
    public float fadeRate = 10;
    public Color healColor;
    public Color hurtColor;

    #endregion References


    #region Flash

    public void Flash(bool isHurt)
    {
        if (!gameObject.activeInHierarchy)
            return;
        
        flashImage.color = isHurt ? hurtColor : healColor;
        flashImage.color = new Color(flashImage.color.r, flashImage.color.g, flashImage.color.b, 1);
        
        StopCoroutine(nameof(FlashFade));
        StartCoroutine(nameof(FlashFade));
    }

    private IEnumerator FlashFade()
    {
        while (flashImage.color.a > 0.1f)
        {
            flashImage.color = Color.Lerp(flashImage.color, Color.clear, Time.deltaTime * fadeRate);
            yield return null;
        }
        flashImage.color = Color.clear;
    }

    #endregion Flash
}
