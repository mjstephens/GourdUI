using UnityEngine;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "New Screen Data", 
        menuName = "GourdUI/Screen Data")]
    public class UIScreenConfigDataTemplate : ScriptableObject
    {
        public UIScreenConfigData data;
    }
}