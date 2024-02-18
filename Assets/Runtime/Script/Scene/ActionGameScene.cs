using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Utilities;
using UnityEngine;

namespace Project
{
    public class ActionGameScene : SceneBase
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneTransitionController.Transition(SceneDefine.Title).Forget();
            }
        }
    }
}
