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
        public SceneAssetLoader AssetLoader { get; private set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="loader">アセットLoader</param>
        /// <param name="sceneData">シーンデータ</param>
        /// <returns></returns>
        public virtual UniTask Initialize(IAssetsLoader loader, object sceneData = null)
        {
            AssetLoader = new SceneAssetLoader(loader);
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// 次のシーンから戻った時
        /// </summary>
        /// <param name="sceneData">シーンデータ</param>
        /// <returns></returns>
        public virtual UniTask BackFromNext(object sceneData = null) => UniTask.CompletedTask;
    }
}
