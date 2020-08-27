using UnityEngine;
using UnityEngine.InputSystem;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "New UI System Data", 
        menuName = "GourdUI/Systems/UI System Data", 
        order = 0)]
    public class GourdUISystemData : ScriptableObject
    {
        public int canvasRenderDepthBase;
        public InputActionAsset uiTriggerInputAsset;
    }
}