using Cysharp.Threading.Tasks;
using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// ActionGameのScene
    /// </summary>
    public class ActionGameScene : SceneBase
    {
        [SerializeField] private ActionGamePresenter presenter;
        [SerializeField] private ActionGameView view;

        private ActionGameModel model;
        
        public override async UniTask Prepare(IAssetsLoader loader, SceneTransitionController sceneTransitionController, object sceneData = null)
        {
            await base.Prepare(loader, sceneTransitionController, sceneData);
            
            // MVP初期化
            model = new ActionGameModel();
            model.Initialize();
            view.Prepare(AssetLoader);
            view.Initialize();
            presenter.Prepare(view, model, AssetLoader);
        }
    }
}
