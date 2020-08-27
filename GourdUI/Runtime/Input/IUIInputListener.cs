using UnityEngine.InputSystem;

namespace GourdUI
{
    /// <summary>
    /// Implement this interface to listen for specific input actions
    /// </summary>
    public interface IUIInputListener
    {
        void OnUIInputAction(InputAction.CallbackContext ctx);
    }
}