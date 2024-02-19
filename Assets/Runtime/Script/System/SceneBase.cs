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

        /// <summary>
        /// 準備
        /// *シーンに入った直後、一回のみ呼び出す処理
        /// </summary>
        /// <param name="loader">アセットLoader</param>
        /// <param name="sceneTransitionController"></param>
        /// <param name="sceneData">シーンデータ</param>
        /// <returns></returns>
        public virtual UniTask Prepare(IAssetsLoader loader, object sceneData = null)
        {
            AssetLoader = new SceneAssetLoader(loader);
            data = sceneData;
            return UniTask.CompletedTask;
        }
        
        /// <summary>
        /// 初期化
        /// *シーンの初期化や全体更新する際に呼び出す
        /// </summary>
        /// <returns></returns>
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
