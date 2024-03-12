using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ActionGame
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;

        private string currentAnimation;
        
        // アニメーターの値
        private static readonly int MoveValue = Animator.StringToHash("MoveValue");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int InAir = Animator.StringToHash("InAir");
        private static readonly int LandOnGround = Animator.StringToHash("LandOnGround");

        public void PlayIdle()
        {
            playerAnimator.SetFloat(MoveValue, 0);
        }

        public void PlayMove(MoveStatus moveStatus)
        {
            if (moveStatus == MoveStatus.Dash)
            {
                playerAnimator.SetFloat(MoveValue, 1f);
                return;
            }

            if (moveStatus == MoveStatus.Run)
            {
                playerAnimator.SetFloat(MoveValue, 0.7f);
                return;
            }
            
            playerAnimator.SetFloat(MoveValue, 0.1f);
        }
        
        public void PlayJump()
        {
            playerAnimator.SetTrigger(Jump);
        }
        
        public void PlayInAir()
        {
            playerAnimator.SetTrigger(InAir);
        }
        
        /// <summary>
        /// 着地
        /// </summary>
        public void PlayLandGround()
        {
            playerAnimator.SetTrigger(LandOnGround);
        }
    }
}
