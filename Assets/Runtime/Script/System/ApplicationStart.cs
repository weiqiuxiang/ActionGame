using System.Collections;
using System.Collections.Generic;
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
            SceneManager.LoadScene("ApplicationSystem", LoadSceneMode.Additive);
        }
    }
}
