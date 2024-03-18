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
        private Tween animationTween;
        
        // アニメーターのParameters
        private static readonly int MoveValue = Animator.StringToHash("MoveValue");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int InAir = Animator.StringToHash("InAir");
        private static readonly int OnGround = Animator.StringToHash("OnGround");
        private static readonly int Dodge = Animator.StringToHash("Dodge");
        private static readonly int Exit = Animator.StringToHash("Exit");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int AttackType = Animator.StringToHash("AttackType");
        
        // layer
        private static readonly int HoldWeaponLayerIndex = 1;

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

        public void PlayAttack(int attackType)
        {
            playerAnimator.SetTrigger(Attack);
            playerAnimator.SetInteger(AttackType, attackType);
        }

        public void SetHoldWeaponLayerWeight(float goalValue)
        {
            float currentValue = 0;
            animationTween?.Kill();
            animationTween = DOTween.To(() => currentValue, value =>
            {
                currentValue = value;
                playerAnimator.SetLayerWeight(HoldWeaponLayerIndex, currentValue);
            }, goalValue, 0.1f);
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
