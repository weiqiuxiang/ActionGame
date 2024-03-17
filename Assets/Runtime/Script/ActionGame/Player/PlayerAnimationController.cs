using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Project.ActionGame
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;

        private string currentAnimation;
        private Tween animationTween;
        
        // アニメーターの値
        private static readonly int MoveValue = Animator.StringToHash("MoveValue");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int InAir = Animator.StringToHash("InAir");
        private static readonly int OnGround = Animator.StringToHash("OnGround");
        private static readonly int Dodge = Animator.StringToHash("Dodge");
        private static readonly int Exit = Animator.StringToHash("Exit");

        public void PlayIdle()
        {
            SetMoveValue(0);
        }

        public void PlayMove(MoveStatus moveStatus)
        {
            if (moveStatus == MoveStatus.Dash)
            {

                SetMoveValue(1);
                return;
            }

            if (moveStatus == MoveStatus.Run)
            {
                SetMoveValue(0.7f);
                return;
            }
            
            SetMoveValue(0.1f);
        }

        private void SetMoveValue(float goalValue)
        {
            playerAnimator.SetFloat(MoveValue, goalValue, 0.1f, Time.deltaTime);
        }
        
        public void PlayJump()
        {
            playerAnimator.SetTrigger(Jump);
        }
        
        public void PlayInAir()
        {
            playerAnimator.SetTrigger(InAir);
        }

        public void PlayDodge()
        {
            playerAnimator.SetTrigger(Dodge);
        }
        
        public void PlayExit()
        {
            playerAnimator.SetTrigger(Exit);
        }
        
        /// <summary>
        /// 着地
        /// </summary>
        public void PlayOnGround()
        {
            playerAnimator.SetTrigger(OnGround);
        }

        private void OnDestroy()
        {
            animationTween?.Kill();
        }
    }
}
