using UIGourd;
using UnityEngine;

namespace GourdUI
{
    [System.Serializable]
    public struct UIViewConfigData
    {
        [Header("Prefab")] 
        public GameObject prefab;

        [Header("Filters")] 
        public UIViewFilterBaseConfigDataTemplate[] filterData;

        [Header("Triggers")] 
        public UITriggerBaseConfigDataTemplate[] viewSpecificTriggers;
    }
}