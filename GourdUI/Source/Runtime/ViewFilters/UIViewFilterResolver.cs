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
            UIViewFilterBaseConfigDataTemplate[] positiveFilters, 
            UIViewFilterBaseConfigDataTemplate[] negativeFilters, 
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
                        case UIViewFilterPlatformConfigDataTemplate f:
                            positiveValid = ViewPlatformFilterIsValid(f, deviceData);
                            break;
                        case UIViewFilterOrientationConfigDataTemplate f:
                            positiveValid = ViewOrientationFilterIsValid(f, deviceData);
                            break;
                        case UIViewFilterInputConfigDataTemplate f:
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
                        case UIViewFilterPlatformConfigDataTemplate f:
                            negativeValid = !ViewPlatformFilterIsValid(f, deviceData);
                            break;
                        case UIViewFilterOrientationConfigDataTemplate f:
                            negativeValid = !ViewOrientationFilterIsValid(f, deviceData);
                            break;
                        case UIViewFilterInputConfigDataTemplate f:
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
            UIViewFilterPlatformConfigDataTemplate filter, 
            AppDeviceData data)
        {
            bool valid;
            switch (data.platform)
            {
                case CoreDevice.DevicePlatform.Desktop:
                    valid = filter.data.desktop;
                    break;
                case CoreDevice.DevicePlatform.Console:
                    valid = filter.data.console;
                    break;
                case CoreDevice.DevicePlatform.Mobile:
                    valid = filter.data.mobile;
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
            UIViewFilterOrientationConfigDataTemplate filter, 
            AppDeviceData data)
        {
            bool valid;
            switch (data.orientation)
            {
                case CoreDevice.DeviceOrientation.Landscape:
                    valid = filter.data.landscape;
                    break;
                case CoreDevice.DeviceOrientation.Portrait:
                    valid = filter.data.portrait;
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
            UIViewFilterInputConfigDataTemplate filter,
            AppDeviceData data)
        {
            bool valid;
            switch (data.input)
            {
                case CoreDevice.DeviceInput.MouseKB:
                    valid = filter.data.mouseKB;
                    break;
                case CoreDevice.DeviceInput.Controller:
                    valid = filter.data.controller;
                    break;
                case CoreDevice.DeviceInput.Touchscreen:
                    valid = filter.data.touchscreen;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return valid;
        }

        #endregion Filter
    }
}