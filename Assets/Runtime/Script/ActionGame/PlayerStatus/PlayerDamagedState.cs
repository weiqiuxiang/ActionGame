using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ActionGame
{
    /// <summary>
    /// 攻撃を受けた
    /// </summary>
    public class PlayerDamagedState : PlayerStateBase
    {
        private float currentTime;
        
        public PlayerDamagedState(PlayerController playerController) : base(playerController)
        {
        }

        public override void InStatus(PlayerState previousState, PlayerStateData receiveData)
        {
            currentTime = receiveData.second;
        }

        public override PlayerState Update()
        {
            // 硬直時間完了までカウントダウン
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                return PlayerState.None;
            }

            // 空中
            if (!playerController.IsOnGround)
            {
                animationController.PlayJumpIdle();
                NextStateData.forward = playerController.GetPlayerForward();
                return PlayerState.InAir;
            }

            // ダメージ受け
            if (playerController.IsDamaged)
            {
                return PlayerState.Damaged;
            }

            // 回避
            if (playerController.IsInputDodge)
            {
                NextStateData.forward = playerController.GetInputForward();
                return PlayerState.Dodge;
            }

            // 攻撃
            if (playerController.IsInputAttack)
            {
                NextStateData.forward = cameraController.IsLockOn? 
                    cameraController.VectorToTarget(true).normalized : playerController.GetInputForward();
                return PlayerState.Attack; 
            }
            
            // 何も起きない場合、Idleに戻る
            return PlayerState.IdleAndMove;
        }
    }
}
