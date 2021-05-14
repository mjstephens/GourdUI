namespace GourdUI
{
    public interface IUIScreen : IBaseUIElement
    {
        void OnScreenSetStackOrder(int index);
        void OnAppDeviceDataUpdated(AppDeviceData deviceData);
        UIScreenConfigData ScreenConfigData();
    }
}