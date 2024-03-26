using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.ActionGame
{
    /// <summary>
    /// キャラのアニメーション再生をコントロール
    /// </summary>
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        private Tween animationTween;

        private CancellationTokenSource animationListPlayCancelToken;
        
        // アニメーターのParameters
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Walk = Animator.StringToHash("Walk");
        public static readonly int Run = Animator.StringToHash("Run");
        public static readonly int Dash = Animator.StringToHash("Dash");
        public static readonly int IdleAndMove = Animator.StringToHash("IdleAndMove");
        public static readonly int MoveX = Animator.StringToHash("MoveX");
        public static readonly int MoveY = Animator.StringToHash("MoveY");
        
        public static readonly int JumpReady = Animator.StringToHash("JumpReady");
        public static readonly int JumpUp = Animator.StringToHash("JumpUp");
        public static readonly int JumpIdle = Animator.StringToHash("JumpIdle");
        public static readonly int JumpLand = Animator.StringToHash("JumpLand");
        
        public static readonly int Dodge = Animator.StringToHash("Dodge");

        private static readonly float walkThrethod = 0.4f;
        private static readonly float runThrethod = 0.8f;
        private static readonly float moveValueChangeSpeed = 5f;
        private Vector2 moveValue = Vector2.zero;
        private Vector2 moveValueGoal = Vector2.zero;

        private readonly Dictionary<string, int> runtimeStringToHashCache = new Dictionary<string, int>();

        private int currentState = 0;
        private bool isPlayingAnimationList = false;
        private int[] interruptStates;
        private bool isReverseInterruptMode = false;
        
        private static readonly int HoldWeaponLayerIndex = 1;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            currentState = Idle;
            moveValue = Vector2.zero;
        }

        private string GetAttackAnimationName(PlayerAttackData attackData) => $"Attack{attackData.Label}";
        
        public bool IsState(int stateHash) => currentState == stateHash;
        public bool IsAnimationFinish(int stateHash)
        {
            if (!IsState(stateHash)) return false;
            var info = playerAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateHash != info.shortNameHash) return false;  // アニメーター側まだ切り替え終わってない
            return info.normalizedTime >= 1;
        }

        public void CrossFadeTo(string animationName, float transitionDuration = 0.2f)
        {
            if (!runtimeStringToHashCache.ContainsKey(animationName))
            {
                runtimeStringToHashCache.Add(animationName, Animator.StringToHash(animationName));
            }
            int state = runtimeStringToHashCache[animationName];
            CrossFadeTo(state, transitionDuration);
        }
        
        public void CrossFadeTo(int stateHash, float transitionDuration = 0.2f)
        {
            // 連続再生アニメーション中断判定
            if (isPlayingAnimationList)
            {
                bool isInterrupt = false;
                if (isReverseInterruptMode)
                {
                    isInterrupt = !interruptStates.Contains(stateHash);
                }
                else
                {
                    isInterrupt = interruptStates.Contains(stateHash);
                }
                
                if (isInterrupt)
                {
                    animationListPlayCancelToken?.Cancel();
                    isPlayingAnimationList = false;
                }
                else
                {
                    return;
                }
            }
            
            if (currentState == stateHash) return;
            currentState = stateHash;
            playerAnimator.CrossFadeInFixedTime(stateHash, transitionDuration);
        }

        public void PlayAnimationList(int[] stateHashes, int[] interruptStates = null, bool isReverseInterruptMode = false)
        {
            this.interruptStates = interruptStates;
            this.isReverseInterruptMode = isReverseInterruptMode;
            if (interruptStates == null) this.interruptStates = Array.Empty<int>();
            PlayAnimationListAsync(stateHashes).Forget();
        }
        
        public void PlayAnimationListAnyStateInterrupt(int[] stateHashes, int[] interruptStates = null)
        {
            this.interruptStates = interruptStates;
            if (interruptStates == null) this.interruptStates = Array.Empty<int>();
            PlayAnimationListAsync(stateHashes).Forget();
        }

        private async UniTask PlayAnimationListAsync(int[] stateHashes)
        {
            animationListPlayCancelToken?.Dispose();
            animationListPlayCancelToken = new CancellationTokenSource();
            isPlayingAnimationList = true;
            // 配列のアニメーションを順番に再生
            for (int i = 0; i < stateHashes.Length; i++)
            {
                int stateHash = stateHashes[i];
                currentState = stateHash;
                playerAnimator.CrossFadeInFixedTime(stateHash, 0.2f);
                await UniTask.WaitUntil(() => IsAnimationFinish(stateHash), cancellationToken:animationListPlayCancelToken.Token);
            }
            isPlayingAnimationList = false;
        }

        public void PlayMove(MoveStatus status, Vector2 moveDirection)
        {
            switch (status)
            {
                case MoveStatus.Dash:
                    moveValueGoal = Vector2.one;
                    CrossFadeTo(Dash);
                    break;
                case MoveStatus.Idle:
                    moveValueGoal = Vector2.zero;
                    CrossFadeTo(IdleAndMove);
                    break;
                case MoveStatus.Run:
                    moveValueGoal.x = runThrethod * moveDirection.x;
                    moveValueGoal.y = runThrethod * moveDirection.y;
                    CrossFadeTo(IdleAndMove);
                    break;
                case MoveStatus.Walk:
                    moveValueGoal.x = walkThrethod * moveDirection.x;
                    moveValueGoal.y = walkThrethod * moveDirection.y;
                    CrossFadeTo(IdleAndMove);
                    break;
            }
        }
        
        public void PlayJumpReady()
        {
            CrossFadeTo(JumpReady);
        }
        
        public void PlayJumpUp()
        {
            CrossFadeTo(JumpUp);
        }
        
        public void PlayJumpIdle()
        {
            CrossFadeTo(JumpIdle, 0.2f);
        }
        
        public void PlayJumpLand()
        {
            CrossFadeTo(JumpLand);
        }

        public void PlayDodge()
        {
            CrossFadeTo(Dodge);
        }

        public void PlayAttack(PlayerAttackData attackData)
        {
            CrossFadeTo(GetAttackAnimationName(attackData));
        }

        public void SetHoldWeaponLayerWeight(float goalValue)
        {
            float currentValue = 0;
            animationTween?.Kill();
            animationTween = DOTween.To(() => currentValue, value =>
            {
                currentValue = value;
                playerAnimator.SetLayerWeight(HoldWeaponLayerIndex, currentValue);
            }, goalValue, 1f);
        }

        private void Update()
        {
            moveValue.x = Mathf.Lerp(moveValue.x, moveValueGoal.x, moveValueChangeSpeed * Time.deltaTime);
            moveValue.y = Mathf.Lerp(moveValue.y, moveValueGoal.y, moveValueChangeSpeed * Time.deltaTime);
            playerAnimator.SetFloat(MoveX, moveValue.x);
            playerAnimator.SetFloat(MoveY, moveValue.y);
        }

        private void OnDestroy()
        {
            animationTween?.Kill();
            animationListPlayCancelToken?.Dispose();
        }
    }
}
