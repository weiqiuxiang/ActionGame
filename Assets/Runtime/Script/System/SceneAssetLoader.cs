using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.System;
using UnityEngine;

namespace Project.System
{
    /// <summary>
    /// シーンで、アセットをロード、解放する機能
    /// シーンはいらない際に、Releaseメソッドでシーン用アセットを解放できる
    /// </summary>
    public class SceneAssetLoader
    {
        private IAssetsLoader loader;
        private readonly List<string> loadedAssetKeyList;
        public SceneAssetLoader(IAssetsLoader loader)
        {
            this.loader = loader;
            loadedAssetKeyList = new List<string>();
        }
        
        public T LoadAsset<T>(string path) where T : Object
        {
            loadedAssetKeyList.Add(path);
            return loader.LoadAsset<T>(path);
        }

        public async UniTask<T> LoadAssetAsync<T>(string path) where T : Object
        {
            loadedAssetKeyList.Add(path);
            return await loader.LoadAssetAsync<T>(path);
        }

        /// <summary>
        /// ロード済みのアセットを解放、SceneAssetLoaderでロードしていないアセットは解放されない
        /// </summary>
        /// <param name="pathList"></param>
        public void ReleaseAssetList(List<string> pathList)
        {
            List<string> containPathList = new List<string>();
            foreach (var path in pathList)
            {
                if (loadedAssetKeyList.Contains(path))
                {
                    containPathList.Add(path);
                    loadedAssetKeyList.Remove(path);
                }
            }
            loader.ReleaseAssetList(containPathList);
        }

        public void Release()
        {
            ReleaseAssetList(loadedAssetKeyList);
        }
    }
}
