using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.System;
using Project.Utilities;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Project.Title
{
    public class TitleScene : SceneBase
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SceneTransitionController.Back().Forget();
            }
        }
    }
}
