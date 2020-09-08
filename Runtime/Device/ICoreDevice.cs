namespace GourdUI
{
    public interface ICoreDevice
    {
        /// <summary>
        /// Monitors for device changes.
        /// </summary>
        void CheckForDeviceUpdates();
        
        /// <summary>
        /// Returns the current app device data.
        /// </summary>
        /// <returns>The current data for the app device.</returns>
        AppDeviceData DeviceData();

        /// <summary>
        /// Registers a screen update listener to respond to safe area changes.
        /// </summary>
        /// <param name="screenRectUpdateListener">The IScreenRectUpdateListener component to be registered.</param>
        void RegisterScreenUpdateListener(IScreenRectUpdateListener screenRectUpdateListener);

        /// <summary>
        /// Unregisters a UI screen update listener.
        /// </summary>
        /// <param name="screenRectUpdateListener">The IScreenRectUpdateListener component to be unregistered.</param>
        void UnregisterScreenUpdateListener(IScreenRectUpdateListener screenRectUpdateListener);
    }
}