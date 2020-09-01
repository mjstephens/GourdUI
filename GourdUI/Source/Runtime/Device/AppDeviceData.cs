namespace GourdUI
{
    /// <summary>
    /// Holds information about the current state of the device.
    /// </summary>
    public struct AppDeviceData
    {
        public CoreDevice.DevicePlatform platform;
        public CoreDevice.DeviceOrientation orientation;
        public CoreDevice.DeviceInput input;
    }
}