using GourdUI;

public interface IUIContractView<in S> where S : UIState
{
    void OnViewInstantiated();
    void ApplyScreenStateToView(S state);
    void OnDestroyView();
}