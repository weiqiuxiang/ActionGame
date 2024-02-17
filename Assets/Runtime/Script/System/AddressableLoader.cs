using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

        public void ReleaseAsset(List<string> pathList)
        {
            foreach (var path in pathList)
            {
                if (!handles.ContainsKey(path) || handles[path].Count == 0) continue;
                Addressables.Release(handles[path][0]);
                handles[path].RemoveAt(0);
                
                // 参照数が0
                if (handles[path].Count == 0)
                {
                    handles.TryRemove(path, out List<AsyncOperationHandle> list);
                }
            }
        }
    }
}
