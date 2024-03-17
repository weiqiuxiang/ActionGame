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

        public override void InStatus(PlayerStatus lastStatus, NextStateData data)
        {
            airForward = data.forward;
        }

        public override PlayerStatus FixedUpdate()
        {
            playerController.AirMove(airForward);
            return PlayerStatus.None;
        }

        public override PlayerStatus Update()
        {
            if (playerController.IsOnGround)
            {
                playerController.AnimationController.PlayOnGround();
                return PlayerStatus.IdleAndMove;
            }
            return PlayerStatus.None;
        }
    }
}
