using UnityEngine;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "Input View Filter", 
        menuName = "GourdUI/Filters/Input Filter")]
    public class UIViewFilterInputConfigData : UIViewFilterBaseConfigData
    {
        public bool mouseKB;
        public bool controller;
        public bool touchscreen;
    }
}