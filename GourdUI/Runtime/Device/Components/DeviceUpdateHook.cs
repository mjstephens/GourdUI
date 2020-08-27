using UnityEngine;

namespace GourdUI
{
    public class DeviceUpdateHook : MonoBehaviour
    {
        #region Singleton

        public static DeviceUpdateHook Instance;

        #endregion Singleton


        #region Initialization

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion Initialization


        #region Update

        private void LateUpdate()
        {
            GourdUI.Device.CheckForDeviceUpdates();
        }

        #endregion Update
    }
}