using System.Collections;
using System.Collections.Generic;
using Project.System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    /// <summary>
    /// アプリ起動時の処理
    /// </summary>
    internal static class ApplicationStart
    {
        /// <summary>
        /// ゲーム起動時の処理
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void OnGameStart()
        {
            Scene startScene = SceneManager.GetActiveScene();
            var asyncOperation = SceneManager.LoadSceneAsync("ApplicationSystem", LoadSceneMode.Additive);
            asyncOperation.completed += _ => OnApplicationSystemSceneLoaded(startScene);
        }

        /// <summary>
        /// ApplicationSystemロード完了時の処理
        /// </summary>
        private static void OnApplicationSystemSceneLoaded(Scene startScene)
        {
            ApplicationSystemScene applicationSystemScene = Object.FindObjectOfType<ApplicationSystemScene>(true);
            if (applicationSystemScene == null)
            {
                UnityEngine.Debug.LogError("ApplicationSystemシーンは存在しない。初期化できない");
                return;
            }
            
            applicationSystemScene.Prepare(startScene);
        }
    }
}
