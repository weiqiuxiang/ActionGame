using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.System
{
    /// <summary>
    /// アセットのロード、解放用interface
    /// </summary>
    public interface IAssetsLoader
    {
        public T LoadAsset<T>(string path) where T : Object;
        public UniTask<T> LoadAssetAsync<T>(string path) where T : Object;
        public void ReleaseAsset(List<string> pathList);
    }
}
