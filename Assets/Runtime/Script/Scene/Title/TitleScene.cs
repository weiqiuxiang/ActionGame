using Cysharp.Threading.Tasks;
using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// TitleのScene
    /// </summary>
    public class TitleScene : SceneBase
    {
        [SerializeField] private TitlePresenter presenter;
        [SerializeField] private TitleView view;

        private TitleModel model;
        
        public override async UniTask Prepare(IAssetsLoader loader, object sceneData = null)
        {
            await base.Prepare(loader, sceneData);
            
            // MVP初期化
            model = new TitleModel();
            model.Initialize();
            view.Prepare(AssetLoader);
            view.Initialize();
            presenter.Prepare(view, model, AssetLoader);
        }
    }
}
