using System;
using UnityEngine;

namespace Project.ActionGame
{
    [Serializable]
    public class PlayerAttackData
    {
        [SerializeField] private float second;  // 持続時間
        public float Second => second;
        [SerializeField] private float moveSpeed;   // 攻撃時移動速度
        public float MoveSpeed => moveSpeed; 
        [SerializeField] private AnimationCurve moveSpeedCurve; // 移動速度変化曲線
        public AnimationCurve MoveSpeedCurve => moveSpeedCurve; 
        [SerializeField] private float startCancelPercent;  // 回避にキャンセルできる開始時間割合
        public float StartCancelPercent => startCancelPercent;
        [SerializeField] private float endCancelPercent;      // 回避にキャンセルできる終了時間割合
        public float EndCancelPercent => endCancelPercent;
        [SerializeField] private float toNextAttackPercent;      // 次の攻撃につなぐ時間割合
        public float ToNextAttackPercent => toNextAttackPercent;
    }
    
    /// <summary>
    /// プレイヤーの設定データ
    /// </summary>
    [CreateAssetMenu(menuName = "ProjectTool/ActionGameSettings/PlayerAttackSettings", fileName = "PlayerAttackSettings")]
    public class PlayerAttackSettings : ScriptableObject
    {
        [SerializeField] private float attackRotateSpeed;
        public float AttackRotateSpeed => attackRotateSpeed;
        
        [SerializeField] private PlayerAttackData[] attackData;
        public PlayerAttackData[] AttackData => attackData;
    }
}
