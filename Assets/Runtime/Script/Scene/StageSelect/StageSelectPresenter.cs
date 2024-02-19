using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// StageSelectのPresenter
    /// </summary>
    public class StageSelectPresenter : MonoBehaviour
    {
        private StageSelectView view;
        private StageSelectModel model;
        private SceneAssetLoader loader;
        
        public void Prepare(StageSelectView view, StageSelectModel model, SceneAssetLoader loader)
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
