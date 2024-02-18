using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.System
{
    /// <summary>
    /// シーン遷移をコントロール
    /// </summary>
    public class SceneTransitionController
    {
        private readonly List<SceneDefine> history = new List<SceneDefine>();
        public SceneDefine CurrentSceneDefine { get; private set; }
        private Scene currentScene;
        private SceneBase currentSceneBase;
        private readonly IAssetsLoader loader;
        public bool HasBackScene => history.Count > 1;  //戻れるシーンはあるか？
        public SceneDefine BackSceneDefine => history[^2];  // 戻れるシーンのSceneDefineを取得(*HasBackSceneはtrueでないと、エラー吐く)
        
        public SceneTransitionController(IAssetsLoader loader, Scene startScene, SceneBase startSceneBase)
        {
            this.loader = loader;

            currentScene = startScene;
            currentSceneBase = startSceneBase;
            Enum.TryParse(startScene.name, out SceneDefine define);
            CurrentSceneDefine = define;
            history.Add(CurrentSceneDefine);

            currentSceneBase.Prepare(loader, this);
        }

        /// <summary>
        /// シーン遷移
        /// </summary>
        /// <param name="sceneDefine"></param>
        /// <param name="sceneData"></param>
        /// <param name="isResetHistory"></param>
        public async UniTask Transition(SceneDefine sceneDefine, object sceneData = null, bool isResetHistory = false)
        {
            // シーンをロード
            var result = await LoadScene(sceneDefine);
            if (!result.isSucceeded)
            {
                return;
            }

            // 履歴をリセット
            if (isResetHistory)
            {
                history.Clear();
            }
            
            UnloadCurrentScene();
            SceneManager.UnloadSceneAsync(currentScene);

            // シーン初期化
            currentScene = result.scene;
            CurrentSceneDefine = sceneDefine;
            currentSceneBase = result.sceneBase;
            history.Add(sceneDefine);
            await currentSceneBase.Prepare(loader, this, sceneData);
        }

        /// <summary>
        /// 前のシーンに戻る
        /// </summary>
        /// <param name="sceneData"></param>
        public async UniTask Back(object sceneData = null)
        {
            // 戻れるシーンを取得
            if (!HasBackScene)
            {
                UnityEngine.Debug.LogError("戻れるシーンはない");
                return;
            }
            history.RemoveAt(history.Count - 1);
            SceneDefine sceneDefine = history[^1];
            
            // シーンをロード
            var result = await LoadScene(sceneDefine);
            if (!result.isSucceeded)
            {
                return;
            }
            
            UnloadCurrentScene();
            SceneManager.UnloadSceneAsync(currentScene);
            
            // シーン初期化
            currentScene = result.scene;
            CurrentSceneDefine = sceneDefine;
            currentSceneBase = result.sceneBase;
            await currentSceneBase.Prepare(loader, this, sceneData);
            await currentSceneBase.BackFromNext();  // 前のシーンに戻る際の処理
        }

        private async UniTask<(bool isSucceeded, Scene scene, SceneBase sceneBase)> LoadScene(SceneDefine sceneDefine)
        {
            // 次のシーンをロード
            string scenePath = GetScenePath(sceneDefine);
            Scene scene = await loader.LoadSceneAsync(scenePath);
            
            GameObject sceneObject = scene.GetRootGameObjects().FirstOrDefault(rootObject => rootObject.name == "Scene");    // SceneBaseがアタッチしているオブジェクトの名前は必ず「Scene」になる
            if (sceneObject == null)
            {
                UnityEngine.Debug.LogError($"Sceneという名前のオブジェクトが存在しない、{sceneDefine}のロード失敗");
                loader.ReleaseAsset(scenePath);
                return (false, scene, null);
            }
            
            SceneBase sceneBase = sceneObject.GetComponent<SceneBase>();
            if (sceneBase == null)
            {
                UnityEngine.Debug.LogError($"SceneBaseコンポネントがSceneにアタッチしてない、{sceneDefine}の初期化失敗");
                loader.ReleaseAsset(scenePath);
                return (false, scene, null);
            }

            return (true, scene, sceneBase);
        }

        private void UnloadCurrentScene()
        {
            currentSceneBase.Release();
            loader.ReleaseAsset(GetScenePath(CurrentSceneDefine));
        }

        private string GetScenePath(SceneDefine sceneDefine)
        {
            return $"{sceneDefine.ToString()}/{sceneDefine.ToString()}";
        }
    }
}
