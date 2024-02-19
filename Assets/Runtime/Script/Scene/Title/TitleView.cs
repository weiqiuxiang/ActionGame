using Project.System;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    /// <summary>
    /// TitleのView
    /// </summary>
    public class TitleView : MonoBehaviour
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
