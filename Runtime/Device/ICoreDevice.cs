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
        /// Registers a UI safe area to respond to safe area changes.
        /// </summary>
        /// <param name="safeArea">The SafeArea component to be registered.</param>
        void RegisterSafeAreaComponent(ISafeArea safeArea);

        /// <summary>
        /// Unregisters a UI safe area.
        /// </summary>
        /// <param name="safeArea">The SafeArea component to be unregistered.</param>
        void UnegisterSafeAreaComponent(ISafeArea safeArea);
    }
}