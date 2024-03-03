using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Utilities
{
    /// <summary>
    /// Objectを使いまわしpool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InstantiateObjectPool<T> where T:Object
    {
        private readonly T source;
        private readonly Transform parent;
        private readonly Func<T, bool> onIsCanUse; // インスタンスは使用できるかどうか
        private readonly Action<T> onInitialize; // 初期化
        private readonly Action<T> onNoUse;  // 非使用状態にする
        
        private readonly List<T> list = new List<T>();

        public InstantiateObjectPool(
            [DisallowNull] T source,
            [DisallowNull] Transform parent,
            [DisallowNull] Func<T, bool> onIsCanUse,
            Action<T> onInitialize,
            Action<T> onNoUse)
        {
            this.source = source;
            this.parent = parent;
            this.onIsCanUse = onIsCanUse;
            this.onInitialize = onInitialize;
            this.onNoUse = onNoUse;
        }

        /// <summary>
        /// 未使用状態のインスタンスを取得
        /// </summary>
        /// <returns></returns>
        public T UseOne()
        {
            T instance = list.FirstOrDefault(instance => onIsCanUse(instance));
            
            // 使用できるものはないなら、Instantiateで作る
            if (instance == null)
            {
                instance = Object.Instantiate(source, parent);
                list.Add(instance);
            }
            
            onInitialize?.Invoke(instance);
            return instance;
        }

        public IReadOnlyList<T> GetInstanceList() => list;

        /// <summary>
        /// 全インスタンスを未使用状態にする
        /// </summary>
        public void NoUseAll()
        {
            foreach (var instance in list)
            {
                onNoUse?.Invoke(instance);
            }
        }

        public void DestroyAllInstance()
        {
            foreach (var instance in list)
            {
                Object.Destroy(instance);
            }
            list.Clear();
        }
    }
}
