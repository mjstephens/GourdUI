using GourdUI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIView_Playground_Mobile : UIView<UIScreen_Playground>, IUIContractView_Playground
{
    #region Fields
    
    public DataCollectionUIInstance collectionGridInstancePrefab;
    
    public Button disableScreenButton;
    public Slider value1Slider;
    public TMP_Text value1ValueDisplay;
    public Slider value2Slider;
    public TMP_Text value2ValueDisplay;
    public RectTransform collection1GridParent;
    public RectTransform collection2GridParent;

    #endregion Fields


    #region Setup

    public override void OnViewPreSetup()
    {
        disableScreenButton.onClick.AddListener(delegate { screenContract.OnScreenDisabledFromView(); });
        value1Slider.onValueChanged.AddListener(delegate { screenContract.OnValue1Changed(value1Slider.value); });
        value2Slider.onValueChanged.AddListener(delegate { screenContract.OnValue2Changed(value2Slider.value); });
    }
    
    #endregion Setup


    #region Screen Updates

    public void ReceiveValue1ValueUpdate(float value)
    {
        value1ValueDisplay.text = value.ToString("F2");
        value1Slider.value = value;
    }

    public void ReceiveValue2ValueUpdate(float value)
    {
        value2ValueDisplay.text = value.ToString("F2");
        value2Slider.value = value;
    }

    public void UpdateCollection1Data(UIScreen_Playground.GridEntryDataExample[] data)
    {
        ClearItemGrid(collection1GridParent);
        PopulateCollectionGridData(collection1GridParent, data);
    }

    public void UpdateCollection2Data(UIScreen_Playground.GridEntryDataExample[] data)
    {
        ClearItemGrid(collection2GridParent);
        PopulateCollectionGridData(collection2GridParent, data);
    }

    #endregion Screen Updates
    
    
    #region UI

    private void OnCollectionInstanceSelected(RectTransform colelctionParent,
        UIScreen_Playground.GridEntryDataExample dataItem)
    {
        if (colelctionParent == collection1GridParent)
        {
            screenContract.OnCollection1ItemSelected(dataItem);
        }
        else
        {
            screenContract.OnCollection2ItemSelected(dataItem);
        }
    }

    #endregion
        
        
    #region Utility

    private void ClearItemGrid(RectTransform gridParent)
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void PopulateCollectionGridData(RectTransform gridParent,
        UIScreen_Playground.GridEntryDataExample[] data)
    {
        foreach (var item in data)
        {
            DataCollectionUIInstance d = Instantiate(collectionGridInstancePrefab, gridParent);
            d.SetData(item);

            EventTrigger.Entry entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerClick};
            entry.callback.AddListener( delegate { OnCollectionInstanceSelected(gridParent, item); } );
            d.selectTrigger.triggers.Add(entry);
        }
    }

    #endregion Utility

    
}
