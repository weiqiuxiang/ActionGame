using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.System
{
    /// <summary>
    /// システムシーン、データのロード、シーン遷移などを初期化
    /// *どのシーンから起動しても、必ずこのシーンの初期化が最初実行される
    /// </summary>
    public class ApplicationSystemScene : MonoBehaviour
    {
        private SceneTransitionController sceneTransitionController;
        private IAssetsLoader assetsLoader;
        
        /// <summary>
        /// 準備
        /// </summary>
        public void Prepare(Scene startScene)
        {
            // TODO::マスターデータなどデータ類をロードする
            
            // データローダー初期化
            assetsLoader = new AddressableLoader();
            
            // シーン遷移マネージャーを初期化
            SceneBase startSceneBase = FindStartSceneBase(startScene);
            if (startSceneBase != null)
            {
                sceneTransitionController = new SceneTransitionController(assetsLoader, startScene, startSceneBase);
            }
        }

        /// <summary>
        /// 起動シーンのSceneBaseを取得
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        private SceneBase FindStartSceneBase(Scene scene)
        {
            // SceneBaseがアタッチしているオブジェクトの名前は必ず「Scene」になる
            GameObject sceneObject = scene.GetRootGameObjects().FirstOrDefault(rootObject => rootObject.name == "Scene");
            return sceneObject == null ? null : sceneObject.GetComponent<SceneBase>();
        }
    }
}
