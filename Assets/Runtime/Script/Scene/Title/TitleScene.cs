using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.System;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Project.Title
{
    public class TitleScene : SceneBase
    {
        public override async UniTask Initialize(IAssetsLoader loader, object sceneData = null)
        {
            await base.Initialize(loader, sceneData);
        }
    }
}
