using UIGourd;
using UnityEngine;

namespace GourdUI
{
    [System.Serializable]
    public struct UIViewConfigData
    {
        public GameObject prefab;
        public UIViewFilterConfigurationModule filterData;
        public UITriggerBaseConfigDataTemplate[] viewSpecificTriggers;
    }
}