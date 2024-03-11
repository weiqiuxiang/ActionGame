using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// プレイヤーの設定データ
    /// </summary>
    [CreateAssetMenu(menuName = "ProjectTool/ActionGameSettings/ActionGameEnvironmentSetting", fileName = "ActionGameEnvironmentSetting")]
    public class ActionGameEnvironmentSetting : ScriptableObject
    {
        [SerializeField] private float gravity; //重力
        public float Gravity => gravity;

        [SerializeField] private float fallSpeedMax;
        public float FallSpeedMax => fallSpeedMax;
    }
}
