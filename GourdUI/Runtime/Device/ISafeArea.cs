using UnityEngine;

namespace GourdUI
{
    public interface ISafeArea
    {
        /// <summary>
        /// Updates the safe area of the current device screen.
        /// </summary>
        /// <param name="rect">The new screen Rect.</param>
        void OnUISafeAreaUpdated(Rect rect);
    }
}