using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.ActionGame
{
    public class PlayerInAirState : PlayerStateBase
    {
        private Vector3 airForward; // 空中向き
        
        public PlayerInAirState(PlayerController playerController) : base(playerController)
        {
        }

        public override void InStatus(PlayerState previousState, PlayerStateData receiveData)
        {
            airForward = receiveData.forward;
        }

        public override PlayerState FixedUpdate()
        {
            playerController.AirMove(airForward);
            return PlayerState.None;
        }

        public override PlayerState Update()
        {
            if (playerController.IsOnGround)
            {
                playerController.AnimationController.PlayOnGround();
                return PlayerState.IdleAndMove;
            }
            return PlayerState.None;
        }
    }
}
