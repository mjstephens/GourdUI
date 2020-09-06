using UnityEngine;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "New View Data", 
        menuName = "GourdUI/View Data")]
    public class UIViewConfigDataTemplate : ScriptableObject
    {
        public UIViewConfigData data;
    }
}