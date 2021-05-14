using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    public class CoreDevice : ICoreDevice
    {
        private static RuntimePlatform EditorEquivalentPlatform 
        { 
            get
            {
                #if UNITY_ANDROID
                return RuntimePlatform.Android;
                #elif UNITY_IOS
                return RuntimePlatform.IPhonePlayer;
                #elif UNITY_STANDALONE_OSX
                return RuntimePlatform.OSXPlayer;
                #elif UNITY_STANDALONE_WIN
                return RuntimePlatform.WindowsPlayer;
                #endif
                return Application.platform;
            }
        }
        
        #region Device Data

        public enum DevicePlatform
        {
            Desktop,
            Console,
            Mobile
        }

        public enum DeviceOrientation
        {
            Portrait,
            Landscape
        }

        public enum DeviceInput
        {
            MouseKB,
            Controller,
            Touchscreen
        }
        
        #endregion Device Data


        #region Variables

        /// <summary>
        /// The current device data.
        /// </summary>
        private static AppDeviceData _currentDeviceData;

        /// <summary>
        /// Name for our hook gameobject
        /// </summary>
        private const string CONST_deviceUpdateHookObjectName = "GourdUI_DeviceUpdateHook";
        
        /// <summary>
        /// Property to access the screen safe area.
        /// </summary>
        /// <returns></returns>
        public static Rect UISafeArea () => Screen.safeArea;
        
        /// <summary>
        /// Previous safe area.
        /// </summary>
        private Rect _lastSafeArea = new Rect (0, 0, 0, 0);
        
        /// <summary>
        /// 
        /// </summary>
        private readonly List<IScreenRectUpdateListener> _screenListeners = new List<IScreenRectUpdateListener>();

        #endregion Variables
        
        
        #region Constructor

        public CoreDevice() 
        {
            // Create update hook
            DeviceUpdateHook hook = new GameObject().AddComponent<DeviceUpdateHook>();
            hook.transform.name = CONST_deviceUpdateHookObjectName;
            
            // Set device data
            SetDeviceData(true, true, true);
            
            // Update UI for first time
            GourdUI.Core.OnAppDeviceUpdated(_currentDeviceData);
        }

        #endregion Constructor
        

        #region Updates
        
        void ICoreDevice.CheckForDeviceUpdates()
        {
            // Check to see if the device data has updated since the previous check
            if (SetDeviceData(false, true, true))
            {
                GourdUI.Core.OnAppDeviceUpdated(_currentDeviceData);
            }
            
            // Check safe area
            Rect safeArea = UISafeArea ();
            if (safeArea != _lastSafeArea)
            {
                _lastSafeArea = safeArea;
                foreach (var s in _screenListeners)
                {
                    s.OnScreenRectUpdated(safeArea);
                }
            }
        }

        #endregion Updates


        #region Device

        /// <summary>
        /// Sets the current device data.
        /// </summary>
        /// <returns></returns>
        private static bool SetDeviceData(
            bool doCheckPlatform, 
            bool doCheckOrientation, 
            bool doCheckInput)
        {
            AppDeviceData thisData = _currentDeviceData;
            
            // Check current device
            if (doCheckPlatform)
            {
                thisData.platform = TranslateToDevicePlatform();
            }
            else
            {
                thisData.platform = _currentDeviceData.platform;
            }

            // Check current orientation
            if (doCheckOrientation)
            {
                thisData.orientation = TranslateToDeviceOrientation();
            }
            else
            {
                thisData.orientation = _currentDeviceData.orientation;
            }

            // Check current input specs
            if (doCheckInput)
            {
                thisData.input = TranslateToDeviceInput(thisData.platform);
            }
            else
            {
                thisData.input = _currentDeviceData.input;
            }

            // Has our device data updated at all?
            bool hasChanged = (thisData.platform != _currentDeviceData.platform ||
                               thisData.orientation != _currentDeviceData.orientation ||
                               thisData.input != _currentDeviceData.input);
            if (hasChanged)
            {
                _currentDeviceData = thisData;
                return true;
            }
            return false;
        }

        #endregion Device


        #region Translation

        /// <summary>
        /// Translates application status to DeviceOrientation value.
        /// </summary>
        /// <returns>The current device DeviceOrientation</returns>
        private static DeviceOrientation TranslateToDeviceOrientation()
        {
            DeviceOrientation orientation = DeviceOrientation.Landscape;
            if (Screen.width < Screen.height)
            {
                orientation = DeviceOrientation.Portrait;
            }
            return orientation;
        }

        /// <summary>
        /// Translates application status to DevicePlatform value.
        /// </summary>
        /// <returns>The current device DevicePlatform</returns>
        private static DevicePlatform TranslateToDevicePlatform()
        {
            RuntimePlatform testPlatform = Application.platform;
            if (testPlatform == RuntimePlatform.OSXEditor ||
                testPlatform == RuntimePlatform.WindowsEditor)
            {
                testPlatform = EditorEquivalentPlatform;
            }
            
            DevicePlatform devicePlatform;
            switch (testPlatform)
            {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    devicePlatform = DevicePlatform.Mobile;
                    break;
                
                case RuntimePlatform.Switch:
                    case RuntimePlatform.PS4:
                    case RuntimePlatform.XboxOne:
                    case RuntimePlatform.tvOS:
                    devicePlatform = DevicePlatform.Console;
                    break;
                
                default:
                    devicePlatform = DevicePlatform.Desktop;
                    break;
            }
            return devicePlatform;
        }
        
        /// <summary>
        /// Translates application status to DeviceInput value.
        /// </summary>
        /// <returns>The current device DeviceInput</returns>
        private static DeviceInput TranslateToDeviceInput(DevicePlatform platform)
        {
            DeviceInput deviceInput;
            switch (platform)
            {
                default:
                    deviceInput = DeviceInput.MouseKB;
                    break;
                case DevicePlatform.Console:
                    deviceInput = DeviceInput.Controller;
                    break;
                case DevicePlatform.Mobile:
                    deviceInput = DeviceInput.Touchscreen;
                    break;
            }
            return deviceInput;
        }

        #endregion Translation


        #region Registration

        void ICoreDevice.RegisterScreenUpdateListener(IScreenRectUpdateListener screenRectUpdateListener)
        {
            _screenListeners.Add(screenRectUpdateListener);
        }

        void ICoreDevice.UnregisterScreenUpdateListener(IScreenRectUpdateListener screenRectUpdateListener)
        {
            _screenListeners.Remove(screenRectUpdateListener);
        }

        #endregion Registration
        
        
        #region Getters

        public AppDeviceData DeviceData()
        {
            return _currentDeviceData;
        }

        #endregion Getters
    }
}