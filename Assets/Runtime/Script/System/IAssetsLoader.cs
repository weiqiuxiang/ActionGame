using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.System
{
    /// <summary>
    /// アセットのロード、解放用interface
    /// </summary>
    public interface IAssetsLoader
    {
        public T LoadAsset<T>(string path) where T : Object;
        public UniTask<T> LoadAssetAsync<T>(string path) where T : Object;
        public UniTask<Scene> LoadSceneAsync(string path);
        public bool ReleaseAsset(string path);
        public void ReleaseAssetList(List<string> pathList);
    }
}
