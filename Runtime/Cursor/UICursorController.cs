using UnityEngine;

namespace GourdUI
{
    public static class UICursorController
    {
        #region Cursor
    
        /// <summary>
        /// Updates the state of the mouse cursor.
        /// </summary>
        /// <param name="isActive">Whether the cursor should become visible (true) or be disabled (false).</param>
        public static void UpdateCursor (bool isActive)
        {
            if (isActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    
        #endregion Cursor
    }
}