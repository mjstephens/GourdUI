namespace GourdUI
{
    public interface IGourdUI
    {
        /// <summary>
        /// Registers a UIScreen instance with the AppUI controller.
        /// </summary>
        /// <param name="screen">The UIScreen instance to register.</param>
        void RegisterScreen(UIScreen screen);

        /// <summary>
        /// Unregisters a UIScreen instance from the AppUI controller.
        /// </summary>
        /// <param name="screen">The UIScreen instance to unregister.</param>
        void UnregisterScreen(UIScreen screen);

        /// <summary>
        /// Toggles a UIScreen on/off based on its current state.
        /// </summary>
        /// <param name="screenTriggerCode">The trigger code of the UIScreen to be toggled.</param>
        void ToggleUIScreen(string screenTriggerCode);

        /// <summary>
        /// Adds a UIScreen to the active screen stack.
        /// </summary>
        /// <param name="screen">The UIScreen to be added to the stack.</param>
        /// <param name="data">Optional data passed into the UIScreen on activation.</param>
        /// <typeparam name="T">Optional data type.</typeparam>
        void AddScreenToStack<T>(UIScreen screen, T data = default);

        /// <summary>
        /// Removes a UIScreen from the active screen stack.
        /// </summary>
        /// <param name="screen">The UIScreen to be removed from the stack.</param>
        void RemoveScreenFromStack(UIScreen screen);

        /// <summary>
        /// Responds to app device data updates.
        /// </summary>
        /// <param name="deviceData">The updated device data.</param>
        void OnAppDeviceUpdated(CoreDevice.AppDeviceData deviceData);

        /// <summary>
        /// Returns true if the given UIView is valid for the current device settings.
        /// </summary>
        /// <param name="viewData"></param>
        /// <returns></returns>
        bool UIViewIsValidForDevice(UIViewConfigData viewData);
    }
}