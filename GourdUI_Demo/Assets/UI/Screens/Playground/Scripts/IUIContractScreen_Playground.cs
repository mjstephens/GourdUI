/// <summary>
/// Defines implementation requirements for the screen from the view.
/// </summary>
public interface IUIContractScreen_Playground : IUIContractScreen
{
    void OnScreenDisabledFromView();
    void OnValue1Changed(float value);
    void OnValue2Changed(float value);
    void OnCollection1ItemSelected(UIScreen_Playground.GridEntryDataExample item);
    void OnCollection2ItemSelected(UIScreen_Playground.GridEntryDataExample item);
}