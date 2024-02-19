using Project.System;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    /// <summary>
    /// ActionGameのView
    /// </summary>
    public class ActionGameView : MonoBehaviour
    {
        [SerializeField] private Button button;
        public Button Button => button;
        
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
