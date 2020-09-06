using UnityEngine;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "Screen Orientation View Filter", 
        menuName = "GourdUI/Filters/Screen Orientation Filter")]
    public class UIViewFilterOrientationConfigData : UIViewFilterBaseConfigData
    {
        public bool portrait;
        public bool landscape;
    }
}