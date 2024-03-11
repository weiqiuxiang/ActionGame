using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// プレイヤーの設定データ
    /// </summary>
    [CreateAssetMenu(menuName = "ProjectTool/ActionGameSettings/PlayerSettings", fileName = "PlayerSettings")]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField] private float moveInputDeadZone = 0.01f;   // 移動入力デッドゾーン
        public float MoveInputDeadZone => moveInputDeadZone;
        
        [SerializeField] private float walkSpeed;   // 歩き速度
        public float WalkSpeed => walkSpeed;
        
        [SerializeField] private float runSpeed;    // 走り速度
        public float RunSpeed => runSpeed;

        [SerializeField] private float dashSpeed;    // ダッシュ速度
        public float DashSpeed => dashSpeed;
        
        [SerializeField] private float fallStartSpeed; // ジャンプなしの落下の初速度
        public float FallStartSpeed => fallStartSpeed;
        
        [SerializeField] private float walkInputThreshold;  // 歩き入力閾値、入力はこの値より小さい時は歩き、この値より大きい時は走り
        public float WalkInputThreshold => walkInputThreshold;

        [SerializeField] private float dodgeSpeed;  // 回避の移動速度
        public float DodgeSpeed => dodgeSpeed;

        [SerializeField] private AnimationCurve dodgeSpeedCurve;    // 回避速度の変化曲線
        public AnimationCurve DodgeSpeedCurve => dodgeSpeedCurve;
        
        [SerializeField] private float dodgeSecond;   // 回避時間
        public float DodgeSecond => dodgeSecond;
        
        [SerializeField] private float dodgeInvincibleSecond;   // 回避無敵時間
        public float DodgeInvincibleSecond => dodgeInvincibleSecond;

        [SerializeField] private float jumpHeight;  //  ジャンプ高さ
        public float JumpHeight => jumpHeight;

        [SerializeField] private float airMoveAcc;  // 空中加速度
        public float AirMoveAcc => airMoveAcc;
    }
}
