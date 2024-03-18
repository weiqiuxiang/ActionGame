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
        
        public override void InStatus(PlayerState previousState, PlayerStateData receiveData)
        {
            dodgeForward = receiveData.forward;
            
            currentTime = 0;
            playerController.PlayerForwardEqualInputVectorFromCamera();
            playerController.AnimationController.PlayDodge();
        }

        public override void OutStatus()
        {
            base.OutStatus();
            playerController.AnimationController.PlayExit();
        }

        public override PlayerState FixedUpdate()
        {
            dodgeForward = playerController.Dodge(currentTime, dodgeForward);
            return PlayerState.None;
        }

        public override PlayerState Update()
        {
            currentTime += Time.deltaTime;
            
            if (!playerController.IsOnGround)
            {
                playerController.AnimationController.PlayInAir();
                NextStateData.forward = playerController.GetPlayerForward();
                return PlayerState.InAir;
            }

            if (currentTime >= playerController.PlayerSettings.DodgeSecond)
            {
                return PlayerState.IdleAndMove;
            }

            // ダメージ受け
            if (playerController.IsDamaged)
            {
                return PlayerState.Damaged;
            }

            // 攻撃
            if (playerController.IsInputAttack)
            {
                //return PlayerStatus.Attack; 
            }

            return PlayerState.None;
        }
    }
}
