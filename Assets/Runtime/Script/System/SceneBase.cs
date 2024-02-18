using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// シーンのBaseクラス
    /// </summary>
    public abstract class SceneBase : MonoBehaviour
    {
        protected object data;
        public object Data => data;
        public SceneAssetLoader AssetLoader { get; private set; }
        public SceneTransitionController SceneTransitionController { get; private set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="loader">アセットLoader</param>
        /// <param name="sceneTransitionController"></param>
        /// <param name="sceneData">シーンデータ</param>
        /// <returns></returns>
        public virtual UniTask Prepare(IAssetsLoader loader, SceneTransitionController sceneTransitionController, object sceneData = null)
        {
            AssetLoader = new SceneAssetLoader(loader);
            SceneTransitionController = sceneTransitionController;
            data = sceneData;
            return UniTask.CompletedTask;
        }
        
        public virtual UniTask Initialize() => UniTask.CompletedTask;

        /// <summary>
        /// 次のシーンから戻った時
        /// </summary>
        /// <returns></returns>
        public virtual UniTask BackFromNext()
        {
            return UniTask.CompletedTask;
        }

        public virtual void Release()
        {
            AssetLoader.Release();
        }
    }
}
