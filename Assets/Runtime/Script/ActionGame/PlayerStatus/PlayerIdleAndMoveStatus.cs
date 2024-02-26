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
        public PlayerIdleAndMoveStatus(PlayerAnimationController animationController, PlayerController playerController) : base(animationController, playerController)
        {
        }

        public override void InStatus(PlayerStatus lastStatus)
        {
            playerController.ResetInputValues();
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
            if (playerController.IsInputJump || !playerController.IsOnGround)
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
