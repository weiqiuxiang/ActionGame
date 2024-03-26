using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ActionGame
{
    public class PlayerAttackState : PlayerStateBase
    {
        private int comboIndex = 0;
        private int comboIndexMax = 0;
        private bool isToDodge = false;
        private bool isToNextAttack = false;
        private Vector3 attackForward;
        private Vector3 nextAttackForward;
        private Vector3 nextDodgeForward;
        private float currentSecond;
        private float currentSecondCount;
        private float timePercent;
        private PlayerAttackData currentAttackData;

        public PlayerAttackState(PlayerController playerController) : base(playerController)
        {
        }

        public override void InStatus(PlayerState previousState, PlayerStateData receiveData)
        {
            attackForward = receiveData.forward;
            
            comboIndex = 0;
            comboIndexMax = playerController.PlayerAttackSettings.ComboRoute.Length - 1;
            currentAttackData = previousState == PlayerState.Dodge?
                playerController.PlayerAttackSettings.GetDodgeAttackData() : 
                playerController.PlayerAttackSettings.GetAttackDataByComboRouteIndex(comboIndex);
            
            ResetNextAction();
            SetTime();
            
            animationController.PlayAttack(currentAttackData);
            animationController.SetHoldWeaponLayerWeight(0);
        }

        public override void OutStatus()
        {
            animationController.SetHoldWeaponLayerWeight(1);
        }

        public override PlayerState FixedUpdate()
        {
            playerController.Attack(currentAttackData, timePercent, attackForward);
            return PlayerState.None;
        }
        
        public override PlayerState Update()
        {
            currentSecondCount += Time.deltaTime;
            timePercent = currentSecondCount / currentSecond;
            UpdateNextAction();
            
            if (timePercent >= 1)
            {
                if (isToNextAttack && comboIndex != comboIndexMax)
                {
                    comboIndex++;
                    currentAttackData = playerController.PlayerAttackSettings.GetAttackDataByComboRouteIndex(comboIndex);
                    attackForward = nextAttackForward;
                    ResetNextAction();
                    SetTime();
                    
                    animationController.PlayAttack(currentAttackData);
                }
                else
                {
                    return PlayerState.IdleAndMove;
                }
            }
            
            if (!playerController.IsOnGround)
            {
                NextStateData.forward = playerController.GetPlayerForward();
                animationController.PlayJumpIdle();
                return PlayerState.InAir;
            }

            if (isToDodge)
            {
                NextStateData.forward = nextDodgeForward;
                return PlayerState.Dodge;
            }
            
            return PlayerState.None;
        }

        private void UpdateNextAction()
        {
            var data = currentAttackData;
            
            // 回避にキャンセルするかをチェック
            bool hasDodge = (timePercent <= data.StartCancelPercent || timePercent >= data.EndCancelPercent) && playerController.IsInputDodge;
            if (!isToDodge && hasDodge)
            {
                isToDodge = true;
                nextDodgeForward = playerController.GetInputForward();
            }
            
            // 次の攻撃をするかチェック
            bool hasAttack = playerController.IsInputAttack && timePercent >= data.ToNextAttackPercent;
            if (!isToNextAttack && hasAttack)
            {
                isToNextAttack = true;
                nextAttackForward = cameraController.IsLockOn? 
                    cameraController.VectorToTarget(true).normalized : playerController.GetInputForward();
            }
        }

        private void ResetNextAction()
        {
            isToDodge = false;
            isToNextAttack = false;
        }
        
        private void SetTime()
        {
            currentSecond = currentAttackData.Second;
            currentSecondCount = 0;
            timePercent = 0;
        }
    }
}
