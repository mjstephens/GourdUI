using UnityEngine;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "Runtime Platform Filter", 
        menuName = "GourdUI/Filters/Runtime Platform Filter")]
    public class UIViewFilterPlatformConfigDataTemplate : UIViewFilterBaseConfigDataTemplate
    {
        public UIViewFilterPlatformConfigData data;
    }
}