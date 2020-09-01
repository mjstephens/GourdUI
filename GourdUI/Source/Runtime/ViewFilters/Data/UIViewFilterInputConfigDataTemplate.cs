using UnityEngine;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "Input View Filter", 
        menuName = "GourdUI/Filters/Input Filter")]
    public class UIViewFilterInputConfigDataTemplate : UIViewFilterBaseConfigDataTemplate
    {
        public UIViewFilterInputConfigData data;
    }
}