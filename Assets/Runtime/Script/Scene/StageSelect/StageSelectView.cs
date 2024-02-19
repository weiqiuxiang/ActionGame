using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// StageSelectのView
    /// </summary>
    public class StageSelectView : MonoBehaviour
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
