using UnityEngine;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "Runtime Platform Filter", 
        menuName = "GourdUI/Filters/Runtime Platform Filter")]
    public class UIViewFilterPlatformConfigData : UIViewFilterBaseConfigData
    {
        public bool desktop;
        public bool mobile;
        public bool console;
    }
}