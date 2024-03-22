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
        
        public static readonly int JumpReady = Animator.StringToHash("JumpReady");
        public static readonly int JumpUp = Animator.StringToHash("JumpUp");
        public static readonly int JumpIdle = Animator.StringToHash("JumpIdle");
        public static readonly int JumpLand = Animator.StringToHash("JumpLand");
        
        public static readonly int Dodge = Animator.StringToHash("Dodge");

        private static readonly int MaxAttackStateHashes = 10;   // 攻撃ハッシュ配列の最大数
        private int[] attackStateHashes;

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
            
            attackStateHashes = new int[MaxAttackStateHashes];
            for (int i = 0; i < attackStateHashes.Length; i++)
            {
                attackStateHashes[i] = Animator.StringToHash($"Attack{i:D2}");
            }
        }
        
        public bool IsState(int stateHash) => currentState == stateHash;
        public bool IsAnimationFinish(int stateHash)
        {
            if (!IsState(stateHash)) return false;
            var info = playerAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateHash != info.shortNameHash) return false;  // アニメーター側まだ切り替え終わってない
            return info.normalizedTime >= 1;
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

        public void PlayIdle()
        {
            CrossFadeTo(Idle, 0.4f);
        }

        public void PlayMove(MoveStatus moveStatus)
        {
            if (moveStatus == MoveStatus.Dash)
            {
                CrossFadeTo(Dash);
                return;
            }

            if (moveStatus == MoveStatus.Run)
            {
                CrossFadeTo(Run);
                return;
            }
            
            CrossFadeTo(Walk);
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

        public void PlayAttack(int attackType)
        {
            CrossFadeTo(attackStateHashes[attackType]);
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

        private void OnDestroy()
        {
            animationTween?.Kill();
            animationListPlayCancelToken?.Dispose();
        }
    }
}
