using System.Collections;
using System.Collections.Generic;
using Project.ActionGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project
{
    /// <summary>
    /// 地面にいる時の動き、移動、ダッシュなどをコントロール
    /// </summary>
    public class PlayerIdleAndMoveStatus : PlayerStatusBase
    {
        public PlayerIdleAndMoveStatus(PlayerController playerController) : base(playerController)
        {
        }

        public override void InStatus(PlayerStatus lastStatus)
        {
            playerController.ResetInputVectorFromCamera();
        }

        public override void OutStatus()
        {
            
        }

        public override PlayerStatus FixedUpdate()
        {
            playerController.Move();
            return PlayerStatus.None;
        }

        public override PlayerStatus Update()
        {
            // ジャンプか空中
            if (playerController.IsInputJump)
            {
                playerController.Jump();
                return PlayerStatus.InAir;
            }

            if (!playerController.IsOnGround)
            {
                return PlayerStatus.InAir;
            }

            // ダメージ受け
            if (playerController.IsDamaged)
            {
                return PlayerStatus.Damaged;
            }

            // 回避
            if (playerController.IsInputDodge)
            {
                return PlayerStatus.Dodge;
            }

            // 攻撃
            if (playerController.IsInputAttack)
            {
                //return PlayerStatus.Attack; 
            }

            return PlayerStatus.None;
        }
    }
}
