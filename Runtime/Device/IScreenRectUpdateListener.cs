using UnityEngine;

namespace GourdUI
{
    public interface IScreenRectUpdateListener
    {
        /// <summary>
        /// Updates the screen rect of the current device screen.
        /// </summary>
        /// <param name="rect">The new screen Rect.</param>
        void OnScreenRectUpdated(Rect rect);
    }
}