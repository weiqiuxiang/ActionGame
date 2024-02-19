using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// TitleのPresenter
    /// </summary>
    public class TitlePresenter : MonoBehaviour
    {
        private TitleView view;
        private TitleModel model;
        private SceneAssetLoader loader;
        
        public void Prepare(TitleView view, TitleModel model, SceneAssetLoader loader)
        {
            this.view = view;
            this.model = model;
            this.loader = loader;
        }
        
        public void Initialize()
        {
            
        }
    }
}
