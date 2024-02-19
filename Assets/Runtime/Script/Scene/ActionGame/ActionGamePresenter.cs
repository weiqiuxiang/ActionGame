using Cysharp.Threading.Tasks;
using Project.System;
using Project.Utilities;
using UniRx;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// ActionGameのPresenter
    /// </summary>
    public class ActionGamePresenter : MonoBehaviour
    {
        private ActionGameView view;
        private ActionGameModel model;
        private SceneAssetLoader loader;
        
        public void Prepare(ActionGameView view, ActionGameModel model, SceneAssetLoader loader)
        {
            this.view = view;
            this.model = model;
            this.loader = loader;
            
            view.Button.OnClickAsObservable()
                .Subscribe(_ => SceneTransitionController.Instance.Back().Forget())
                .AddTo(this);
        }
        
        public void Initialize()
        {
            
        }
    }
}
