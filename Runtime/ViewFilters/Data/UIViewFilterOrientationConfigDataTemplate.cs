using UnityEngine;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "Screen Orientation View Filter", 
        menuName = "GourdUI/Filters/Screen Orientation Filter")]
    public class UIViewFilterOrientationConfigDataTemplate : UIViewFilterBaseConfigDataTemplate
    {
        public UIViewFilterOrientationConfigData data;
    }
}