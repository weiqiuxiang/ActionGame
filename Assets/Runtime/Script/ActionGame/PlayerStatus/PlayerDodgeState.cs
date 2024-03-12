using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ActionGame
{
    public class PlayerDodgeState : PlayerStatusBase
    {
        private float currentTime;
        
        public PlayerDodgeState(PlayerController playerController) : base(playerController)
        {
        }
        
        public override void InStatus(PlayerStatus lastStatus)
        {
            currentTime = 0;
            playerController.ResetPlayerForward();
        }

        public override void OutStatus()
        {
            
        }

        public override PlayerStatus FixedUpdate()
        {
            currentTime += Time.deltaTime;
            playerController.Dodge(currentTime);
            return PlayerStatus.None;
        }

        public override PlayerStatus Update()
        {
            if (!playerController.IsOnGround)
            {
                playerController.AnimationController.PlayInAir();
                playerController.ResetAirForward();
                return PlayerStatus.InAir;
            }

            if (currentTime >= playerController.PlayerSettings.DodgeSecond)
            {
                return PlayerStatus.IdleAndMove;
            }

            // ダメージ受け
            if (playerController.IsDamaged)
            {
                return PlayerStatus.Damaged;
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
