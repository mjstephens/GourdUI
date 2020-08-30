using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataCollectionUIInstance : MonoBehaviour
{
    public Image backgroundImage;
    public TMP_Text label;
    public EventTrigger selectTrigger;


    public void SetData(UIScreen_Playground.GridEntryDataExample data)
    {
        backgroundImage.color = data.color;
        label.text = data.label;
    }
}
