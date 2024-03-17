using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.ActionGame
{
    public class PlayerDodgeState : PlayerStateBase
    {
        private float currentTime;
        private Vector3 dodgeForward;
        
        public PlayerDodgeState(PlayerController playerController) : base(playerController)
        {
        }
        
        public override void InStatus(PlayerStatus lastStatus, NextStateData data)
        {
            dodgeForward = data.forward;
            
            currentTime = 0;
            playerController.PlayerForwardEqualInputVectorFromCamera();
            playerController.AnimationController.PlayDodge();
        }

        public override void OutStatus()
        {
            playerController.AnimationController.PlayExit();
        }

        public override PlayerStatus FixedUpdate()
        {
            currentTime += Time.deltaTime;
            dodgeForward = playerController.Dodge(currentTime, dodgeForward);
            return PlayerStatus.None;
        }

        public override PlayerStatus Update()
        {
            if (!playerController.IsOnGround)
            {
                playerController.AnimationController.PlayInAir();
                NextStateData.forward = playerController.GetPlayerForward();
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
