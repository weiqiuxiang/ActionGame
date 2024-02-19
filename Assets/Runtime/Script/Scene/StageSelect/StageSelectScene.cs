using Cysharp.Threading.Tasks;
using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// StageSelectのScene
    /// </summary>
    public class StageSelectScene : SceneBase
    {
        [SerializeField] private StageSelectPresenter presenter;
        [SerializeField] private StageSelectView view;

        private StageSelectModel model;
        
        public override async UniTask Prepare(IAssetsLoader loader, object sceneData = null)
        {
            await base.Prepare(loader, sceneData);
            
            // MVP初期化
            model = new StageSelectModel();
            model.Initialize();
            view.Prepare(AssetLoader);
            view.Initialize();
            presenter.Prepare(view, model, AssetLoader);
        }
    }
}
