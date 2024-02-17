using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.System
{
    public interface IAssetsLoader
    {
        public T LoadAsset<T>();
        public T LoadAssetAsync<T>();
    }
}
