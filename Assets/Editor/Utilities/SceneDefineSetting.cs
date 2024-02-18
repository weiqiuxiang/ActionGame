using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Project.Editor
{
    [CreateAssetMenu( menuName = "ProjectTool/CreateSceneDefineSetting", fileName = "SceneDefineSetting" )]
    public class SceneDefineSetting : ScriptableObject
    {
        [SerializeField] private SceneAsset[] sceneAssets;
        public SceneAsset[] SceneAssets => sceneAssets;
    }
}
