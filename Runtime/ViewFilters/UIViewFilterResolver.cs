using System;

namespace GourdUI
{
    public static class UIViewFilterResolver
    {
        #region Filter

        /// <summary>
        /// Compares a filter collection against the current device data.
        /// </summary>
        /// <param name="positiveFilters"></param>
        /// <param name="negativeFilters"></param>
        /// <param name="deviceData"></param>
        public static bool ViewFilterResult(
            UIViewFilterBaseConfigData[] positiveFilters, 
            UIViewFilterBaseConfigData[] negativeFilters, 
            AppDeviceData deviceData)
        {
            bool positiveValid = true;
            if (positiveFilters != null && positiveFilters.Length > 0)
            {
                foreach (var filter in positiveFilters)
                {
                    if (!positiveValid)
                        break;
                    
                    switch (filter)
                    {
                        case UIViewFilterPlatformConfigData f:
                            positiveValid = ViewPlatformFilterIsValid(f, deviceData);
                            break;
                        case UIViewFilterOrientationConfigData f:
                            positiveValid = ViewOrientationFilterIsValid(f, deviceData);
                            break;
                        case UIViewFilterInputConfigData f:
                            positiveValid = ViewInputFilterIsValid(f, deviceData);
                            break;
                    }
                }
            }

            bool negativeValid = true;
            if (negativeFilters != null && negativeFilters.Length > 0)
            {
                foreach (var filter in negativeFilters)
                {
                    if (!negativeValid)
                        break;
                    
                    switch (filter)
                    {
                        case UIViewFilterPlatformConfigData f:
                            negativeValid = !ViewPlatformFilterIsValid(f, deviceData);
                            break;
                        case UIViewFilterOrientationConfigData f:
                            negativeValid = !ViewOrientationFilterIsValid(f, deviceData);
                            break;
                        case UIViewFilterInputConfigData f:
                            negativeValid = !ViewInputFilterIsValid(f, deviceData);
                            break;
                    }
                } 
            }

            return positiveValid && negativeValid;
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
            AppDeviceData data)
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
            AppDeviceData data)
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
            AppDeviceData data)
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