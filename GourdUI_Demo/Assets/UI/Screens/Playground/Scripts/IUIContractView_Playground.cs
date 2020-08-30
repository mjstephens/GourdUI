
// EVents that the view can receive from the screen
public interface IUIContractView_Playground : IUIContractView
{
    void ReceiveValue1ValueUpdate(float value);
    void ReceiveValue2ValueUpdate(float value);
    void UpdateCollection1Data(UIScreen_Playground.GridEntryDataExample[] data);
    void UpdateCollection2Data(UIScreen_Playground.GridEntryDataExample[] data);
}


// Events that the screen can receive from the view