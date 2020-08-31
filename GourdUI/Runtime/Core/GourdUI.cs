using System;
using UnityEngine;

namespace GourdUI
{
    /// <summary>
    /// Utility class for easy access to core interfaces.
    /// Access: GourdUI.Core.(method), GourdUI.Device.(method)
    /// </summary>
    public static class GourdUI
    {
        #region Variables

        /// <summary>
        /// The path for our system data in resources
        /// </summary>
        public const string coreUIDataPath = "GourdUI/Core System Data";

        /// <summary>
        /// Static reference to our core interface.
        /// </summary>
        public static IGourdUI Core;

        /// <summary>
        /// Static reference to our device interface.
        /// </summary>
        public static ICoreDevice Device;

        #endregion Variables


        #region Initialization

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // Load core system data
            GourdUISystemData systemData = Resources.Load<GourdUISystemData>(coreUIDataPath);
            if (systemData == null)
            {
                throw new NullReferenceException(
                    "GourdUI requires system configuration data in the Resources folder!");
            }

            // Create objects
            Core = new GourdUICore(systemData);
            Device = new CoreDevice();
        }

        #endregion Initialization
    }
}