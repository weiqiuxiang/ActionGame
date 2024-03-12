using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ActionGame
{
    public class PlayerInAirStatus : PlayerStatusBase
    {
        public PlayerInAirStatus(PlayerController playerController) : base(playerController)
        {
        }

        public override void InStatus(PlayerStatus lastStatus)
        {
            
        }

        public override PlayerStatus FixedUpdate()
        {
            playerController.AirMove();
            return PlayerStatus.None;
        }

        public override PlayerStatus Update()
        {
            if (playerController.IsOnGround)
            {
                playerController.AnimationController.PlayLandGround();
                return PlayerStatus.IdleAndMove;
            }
            return PlayerStatus.None;
        }
    }
}
