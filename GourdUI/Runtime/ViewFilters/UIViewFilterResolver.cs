using System;

namespace GourdUI
{
    public static class UIViewFilterResolver
    {
        #region Filter

        /// <summary>
        /// Filters a UI view against the current device data.
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="deviceData"></param>
        public static bool ViewFilterResult(UIViewConfigData viewData, CoreDevice.AppDeviceData deviceData)
        {
            // Get filter data
            UIViewFilterInputConfigData inputFilters = viewData.filterData.inputFilterData;
            UIViewFilterOrientationConfigData orientationFilters = viewData.filterData.orientationFilterData;
            UIViewFilterPlatformConfigData platformFilters = viewData.filterData.platformFilterData;

            // All filers must pass validation for the view to be displayed.
            bool valid = ViewPlatformFilterIsValid(platformFilters, deviceData);
            if (valid)
            {
                valid = ViewOrientationFilterIsValid(orientationFilters, deviceData);
            }
            if (valid)
            {
                valid = ViewInputFilterIsValid(inputFilters, deviceData);
            }
            return valid;
        }
        

        /// <summary>
        /// Filters view based on platform.
        /// </summary>
        /// <param name="filter">Filter data for view.</param>
        /// <param name="data">Current device data to filter against.</param>
        /// <returns>Returns true if view passes validation for platform.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown in event of invalid switch.</exception>
        private static bool ViewPlatformFilterIsValid(
            UIViewFilterPlatformConfigData filter, 
            CoreDevice.AppDeviceData data)
        {
            bool valid;
            switch (data.platform)
            {
                case CoreDevice.DevicePlatform.Desktop:
                    valid = filter.desktop;
                    break;
                case CoreDevice.DevicePlatform.Console:
                    valid = filter.console;
                    break;
                case CoreDevice.DevicePlatform.Mobile:
                    valid = filter.mobile;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return valid;
        }
        
        
        /// <summary>
        /// Filters view based on orientation.
        /// </summary>
        /// <param name="filter">Filter data for view.</param>
        /// <param name="data">Current device data to filter against.</param>
        /// <returns>Returns true if view passes validation for orientation.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown in event of invalid switch.</exception>
        private static bool ViewOrientationFilterIsValid(
            UIViewFilterOrientationConfigData filter, 
            CoreDevice.AppDeviceData data)
        {
            bool valid;
            switch (data.orientation)
            {
                case CoreDevice.DeviceOrientation.Landscape:
                    valid = filter.landscape;
                    break;
                case CoreDevice.DeviceOrientation.Portrait:
                    valid = filter.portrait;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return valid;
        }
        
        
        /// <summary>
        /// Filters view based on input device.
        /// </summary>
        /// <param name="filter">Filter data for view.</param>
        /// <param name="data">Current device data to filter against.</param>
        /// <returns>Returns true if view passes validation for input.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown in event of invalid switch.</exception>
        private static bool ViewInputFilterIsValid(
            UIViewFilterInputConfigData filter,
            CoreDevice.AppDeviceData data)
        {
            bool valid;
            switch (data.input)
            {
                case CoreDevice.DeviceInput.MouseKB:
                    valid = filter.mouseKB;
                    break;
                case CoreDevice.DeviceInput.Controller:
                    valid = filter.controller;
                    break;
                case CoreDevice.DeviceInput.Touchscreen:
                    valid = filter.touchscreen;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return valid;
        }

        #endregion Filter
    }
}