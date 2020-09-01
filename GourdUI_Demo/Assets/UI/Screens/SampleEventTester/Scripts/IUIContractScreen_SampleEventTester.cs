/// <summary>
/// Defines implementation requirements for the screen from the view.
/// </summary>
public interface IUIContractScreen_SampleEventTester : IUIContractScreen
{
    void OnViewIncreaseHealth(float amount);
    void OnViewDecreaseHealth(float amount);
    
    void OnViewIncreaseCoins(int amount);
    void OnViewDecreaseCoins(int amount);

    void OnHUDToggled();
}