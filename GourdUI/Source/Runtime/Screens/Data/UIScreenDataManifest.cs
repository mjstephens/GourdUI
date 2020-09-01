using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    [CreateAssetMenu(
        fileName = "New UIScreen Manifest", 
        menuName = "GourdUI/Manifests/UIScreen Manifest")]
    public class UIScreenDataManifest : ScriptableObject
    {
        public List<UIScreenConfigDataTemplate> screens;
    }
}