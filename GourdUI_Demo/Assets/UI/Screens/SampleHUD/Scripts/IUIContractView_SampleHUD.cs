/// <summary>
/// Defines implementation requirements for views from the screen.
/// </summary>
public interface IUIContractView_SampleHUD : IUIContractView<UIState_SampleHUD>
{
    void OnReceiveNewHealthValue(float oldValue, float newValue);
    void OnReceiveNewCoinsValue(int oldValue, int newValue);
}