using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// ActionGameのView
    /// </summary>
    public class ActionGameView : MonoBehaviour
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
