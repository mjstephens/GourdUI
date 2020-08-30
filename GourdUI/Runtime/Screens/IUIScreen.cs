namespace GourdUI
{
    public interface IUIScreen
    {
        void OnScreenInstantiated();
        void OnScreenEnabled<T>(T data = default);
        void OnScreenDisabled();
        void OnScreenSetStackOrder(int index);
        void OnAppDeviceDataUpdated(AppDeviceData deviceData);
        UIScreenConfigData ScreenConfigData();
    }
}