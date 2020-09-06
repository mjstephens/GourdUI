using UIGourd;
using UnityEngine;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "New View Data", 
        menuName = "GourdUI/View Data")]
    public class UIViewConfigData : ScriptableObject
    {
        public GameObject prefab;
        public UIViewFilterConfigurationModule filterData;
    }
}