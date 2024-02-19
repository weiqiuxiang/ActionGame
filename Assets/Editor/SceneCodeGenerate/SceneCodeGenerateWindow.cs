using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace Project.Editor
{
    /// <summary>
    /// シーンコードを生成するEditorWindow
    /// </summary>
    public class SceneCodeGenerateWindow : EditorWindow
    {
        [MenuItem("ProjectTools/GenerateScript/SceneCodeGenerateWindow")]
        private static void Open()
        {
            var window = EditorWindow.CreateWindow<SceneCodeGenerateWindow>();
            window.Initialize();
        }

        private readonly string codeGenerateDefaultPath = "Assets/Runtime/Script/Scene";
        private readonly string sceneTemplatePath = "Assets/Editor/SceneCodeGenerate/SceneCopyTemplate.unity";

        private string path;
        private string sceneName;
        private bool isCreateScene;
        
        private void Initialize()
        {
            path = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + codeGenerateDefaultPath;
            sceneName = "SceneName";
            isCreateScene = false;
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Scene名：");
            sceneName = GUILayout.TextField(sceneName);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("path：");
            GUILayout.Label(path);
            if (GUILayout.Button("..."))
            {
                var filePath = EditorUtility.SaveFilePanel("コードの生成先", codeGenerateDefaultPath, "", ".");
                if (!string.IsNullOrEmpty(filePath))
                {
                    path = filePath;
                }
            }
            GUILayout.EndHorizontal();
            isCreateScene = GUILayout.Toggle(isCreateScene, "scene生成");
            if (GUILayout.Button("生成"))
            {
                GenerateSceneAndCode();
            }
        }

        private void GenerateSceneAndCode()
        {
            if (!Directory.Exists(path))
            {
                UnityEngine.Debug.LogError("指定した保存先は存在しない。Sceneコードを生成できない");
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                EditorUtility.DisplayDialog("Scene名は空", "", "ok");
                return;
            }

            // フォルダ存在しないなら作成
            string folderPath = path + "/" + sceneName;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            CreateScene();
            CreateSceneCode(folderPath);
            CreatePresenterCode(folderPath);
            CreateViewCode(folderPath);
            CreateModelCode(folderPath);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// シーンテンプレートで、シーンをコピー
        /// </summary>
        private void CreateScene()
        {
            if (!isCreateScene)
            {
                return;
            }
            
            string filePath = $"{codeGenerateDefaultPath}/{sceneName}/{sceneName}.unity";
            if (!AssetDatabase.CopyAsset(sceneTemplatePath, filePath))
            {
                UnityEngine.Debug.LogError($"シーンのテンプレートが存在しないため、シーンの作成はできなかった。\npath={sceneTemplatePath}");
            }
        }
        
        /// <summary>
        /// シーンコード生成
        /// </summary>
        /// <param name="folderPath"></param>
        private void CreateSceneCode(string folderPath)
        {
            string code = 
@"using Cysharp.Threading.Tasks;
using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// [T]のScene
    /// </summary>
    public class [T]Scene : SceneBase
    {
        [SerializeField] private [T]Presenter presenter;
        [SerializeField] private [T]View view;

        private [T]Model model;
        
        public override async UniTask Prepare(IAssetsLoader loader, SceneTransitionController sceneTransitionController, object sceneData = null)
        {
            await base.Prepare(loader, sceneTransitionController, sceneData);
            
            // MVP初期化
            model = new [T]Model();
            model.Initialize();
            view.Prepare(AssetLoader);
            view.Initialize();
            presenter.Prepare(view, model, AssetLoader);
        }
    }
}
";
            code = code.Replace("[T]", sceneName);
            
            using (FileStream stream = File.OpenWrite(folderPath + $"/{sceneName}Scene.cs"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(code);
                stream.Write(info, 0, info.Length);
            }
        }

        /// <summary>
        /// Presenterコード生成
        /// </summary>
        /// <param name="folderPath"></param>
        private void CreatePresenterCode(string folderPath)
        {
            string code = 
@"using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// [T]のPresenter
    /// </summary>
    public class [T]Presenter : MonoBehaviour
    {
        private [T]View view;
        private [T]Model model;
        private SceneAssetLoader loader;
        
        public void Prepare([T]View view, [T]Model model, SceneAssetLoader loader)
        {
            this.view = view;
            this.model = model;
            this.loader = loader;
        }
        
        public void Initialize()
        {
            
        }
    }
}
";
            code = code.Replace("[T]", sceneName);
            
            using (FileStream stream = File.OpenWrite(folderPath + $"/{sceneName}Presenter.cs"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(code);
                stream.Write(info, 0, info.Length);
            }
        }
        
        /// <summary>
        /// Viewコード生成
        /// </summary>
        /// <param name="folderPath"></param>
        private void CreateViewCode(string folderPath)
        {
            string code = 
@"using Project.System;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// [T]のView
    /// </summary>
    public class [T]View : MonoBehaviour
    {
        private SceneAssetLoader loader;
        
        public void Prepare(SceneAssetLoader loader)
        {
            this.loader = loader;
        }
        
        public void Initialize()
        {
            
        }
    }
}
";
            code = code.Replace("[T]", sceneName);
            
            using (FileStream stream = File.OpenWrite(folderPath + $"/{sceneName}View.cs"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(code);
                stream.Write(info, 0, info.Length);
            }
        }
        
        /// <summary>
        /// Modelコード生成
        /// </summary>
        /// <param name="folderPath"></param>
        private void CreateModelCode(string folderPath)
        {
            string code = 
@"namespace Project
{
    /// <summary>
    /// [T]のmodel
    /// </summary>
    public class [T]Model
    {
        public [T]Model()
        {
            
        }
        
        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            
        }
    }
}
";
            code = code.Replace("[T]", sceneName);
            
            using (FileStream stream = File.OpenWrite(folderPath + $"/{sceneName}Model.cs"))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(code);
                stream.Write(info, 0, info.Length);
            }
        }
    }
}
