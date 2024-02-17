using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Project.Editor
{
    public static class GenerateScriptUtility
    {
        /// <summary>
        /// SceneDefineスクリプトを作成
        /// </summary>
        [MenuItem("ProjectTools/GenerateScript/CreateSceneDefine")]
        public static void CreateSceneDefineScript()
        {
            // 開始部分
            string script = 
@"
namespace Project.Utilities
{
    /// <summary>
    /// Sceneのenum
    /// </summary>
    public enum SceneDefine
    {
";
            // SceneDefineのenumの中身
            var files = Directory.GetFiles(Path.Combine(Application.dataPath, @"Addressable\Scene"));
            files = files.Where(file => !file.EndsWith(".meta")).ToArray(); // metaを含まない
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileNameWithoutExtension(files[i]);
            }
            foreach (var sceneName in files)
            {
                script += $"\t\t{sceneName},\n";
            }
            script = script.Substring(0, script.Length - 2);  // 最後の改行をなくす

            // 終了部分
            script += 
@"
    }
}
";

            using (FileStream stream = File.OpenWrite(Path.Combine(Application.dataPath, @"Runtime\Script\Utilities\SceneDefine.cs")))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(script);
                stream.Write(info, 0, info.Length);
            }
            
            AssetDatabase.Refresh();
        }
    }
}
