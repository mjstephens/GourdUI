using GourdUI;

public interface IUIContractView<in S> where S : UIState
{
    /// <summary>
    /// Called directly after the view instance has been instantiated
    /// </summary>
    void OnViewInstantiated();
    
    /// <summary>
    /// Should immediately apply screen state to view
    /// </summary>
    /// <param name="state"></param>
    void ApplyScreenStateToView(S state);
    
    /// <summary>
    /// Cleans up and destroys the view instance
    /// </summary>
    void OnDestroyView();
}