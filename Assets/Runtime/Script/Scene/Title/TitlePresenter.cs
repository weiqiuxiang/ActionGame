using Cysharp.Threading.Tasks;
using Project.System;
using Project.Utilities;
using UniRx;
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

            view.Button.OnClickAsObservable()
                .Subscribe(_ => SceneTransitionController.Instance.Transition(SceneDefine.ActionGame).Forget())
                .AddTo(this);
        }
        
        public void Initialize()
        {
            
        }
    }
}
