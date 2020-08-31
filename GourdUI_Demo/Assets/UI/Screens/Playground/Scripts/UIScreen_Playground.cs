using GourdUI;
using UnityEngine;

/// <summary>
/// UIScreen implementation.
/// </summary>
public class UIScreen_Playground : 
    UIScreen <UIScreen_Playground, IUIContractView_Playground, UIState_Playground>, 
    IUIContractScreen_Playground
{
    #region Data

    public struct GridEntryDataExample
    {
        public string label;
        public Color color;
    }
    private GridEntryDataExample[] grid1Data;
    private GridEntryDataExample[] grid2Data;

    #endregion Data
    
    
    #region Setup

    protected override void SetupView()
    {
        // Create temp grid data for demo
        grid1Data = new GridEntryDataExample[10];
        for (int i = 0; i < grid1Data.Length; i++)
        {
            grid1Data[i].label = i.ToString();
            grid1Data[i].color = Random.ColorHSV();
        }
        grid2Data = new GridEntryDataExample[50];
        for (int i = 0; i < grid2Data.Length; i++)
        {
            grid2Data[i].label = i.ToString();
            grid2Data[i].color = Random.ColorHSV();
        }
        
        // Populate grid data
        viewContract.UpdateCollection1Data(grid1Data);
        viewContract.UpdateCollection2Data(grid2Data);
    }

    #endregion Setup


    #region State

    protected override void ResetScreenState()
    {
        state.value1Value = 0;
        state.value2Value = 0;
    }

    protected override void ApplyScreenStateToCurrentView()
    {
        viewContract.ReceiveValue1ValueUpdate(state.value1Value);
        viewContract.ReceiveValue2ValueUpdate(state.value2Value);
    }

    #endregion State


    #region View Events

    public void OnScreenDisabledFromView()
    {
        GourdUI.GourdUI.Core.RemoveScreenFromStack(this);
    }

    public void OnValue1Changed(float value)
    {
        state.value1Value = value;
        viewContract.ReceiveValue1ValueUpdate(value);
    }

    public void OnValue2Changed(float value)
    {
        state.value2Value = value;
        viewContract.ReceiveValue2ValueUpdate(value);
    }

    public void OnCollection1ItemSelected(GridEntryDataExample item)
    {
        Debug.Log(item.label);
    }

    public void OnCollection2ItemSelected(GridEntryDataExample item)
    {
        Debug.Log(item.label);
    }

    #endregion View Events
}
