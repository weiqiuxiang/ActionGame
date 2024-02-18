using System.Collections.Concurrent;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Project.System
{
    /// <summary>
    /// Addressableのロード、解放を管理する
    /// </summary>
    public class AddressableLoader : IAssetsLoader
    {
        private readonly ConcurrentDictionary<string, List<AsyncOperationHandle>> handles;  // valueはListの理由は、Addressableの内部は同じアセットのロードに対し、参照カウンタでカウントしているから
        public AddressableLoader()
        {
            handles = new ConcurrentDictionary<string, List<AsyncOperationHandle>>();
        }
        
        public T LoadAsset<T>(string path) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            handle.WaitForCompletion();
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                UnityEngine.Debug.LogError($"path={path}をロードできなかった");
                return null;
            }
            
            if (handles.ContainsKey(path))
            {
                handles[path].Add(handle);
            }
            else
            {
                handles.TryAdd(path, new List<AsyncOperationHandle>{handle});
            }
            return handle.Result;
        }
        
        public async UniTask<T> LoadAssetAsync<T>(string path) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            await UniTask.WaitUntil(() => handle.IsDone);
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                UnityEngine.Debug.LogError($"path={path}をロードできなかった");
                return null;
            }
            
            if (handles.ContainsKey(path))
            {
                handles[path].Add(handle);
            }
            else
            {
                handles.TryAdd(path, new List<AsyncOperationHandle>{handle});
            }
            return handle.Result;
        }

        public async UniTask<Scene> LoadSceneAsync(string path)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(path, LoadSceneMode.Additive);
            await UniTask.WaitUntil(() => handle.IsDone);
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                UnityEngine.Debug.LogError($"path={path}をロードできなかった");
                return default;
            }
            
            if (handles.ContainsKey(path))
            {
                handles[path].Add(handle);
            }
            else
            {
                handles.TryAdd(path, new List<AsyncOperationHandle>{handle});
            }

            return handle.Result.Scene;
        }
        
        public bool ReleaseAsset(string path)
        {
            if (!handles.ContainsKey(path) || handles[path].Count == 0) return false;
            Addressables.Release(handles[path][0]);
            handles[path].RemoveAt(0);
                
            // 参照数が0
            if (handles[path].Count == 0)
            {
                handles.TryRemove(path, out List<AsyncOperationHandle> list);
            }

            return true;
        }

        public void ReleaseAssetList(List<string> pathList)
        {
            foreach (var path in pathList)
            {
                ReleaseAsset(path);
            }
        }
    }
}
