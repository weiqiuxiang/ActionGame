using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// TitleのView
    /// </summary>
    public class TitleView : MonoBehaviour
    {
        private SceneAssetLoader loader;
        
        public void Prepare(SceneAssetLoader loader)
        {
            this.loader = loader;
        }
        
        public void Initialize()
        {
            
        }
    }
}
