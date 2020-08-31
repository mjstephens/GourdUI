/// <summary>
/// Defines implementation requirements for views from the screen.
/// </summary>
public interface IUIContractView_Playground : IUIContractView
{
    void ReceiveValue1ValueUpdate(float value);
    void ReceiveValue2ValueUpdate(float value);
    void UpdateCollection1Data(UIScreen_Playground.GridEntryDataExample[] data);
    void UpdateCollection2Data(UIScreen_Playground.GridEntryDataExample[] data);
}