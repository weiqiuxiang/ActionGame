using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.ActionGame
{
    /// <summary>
    /// 地面にいる時の動き、移動、ダッシュなどをコントロール
    /// </summary>
    public class PlayerIdleAndMoveState : PlayerStateBase
    {
        /// <summary>
        /// ジャンプは準備動作はあるので、準備動作後ジャンプに遷移
        /// </summary>
        private bool isMoveToJump = false;
        private float moveToJumpTime = 0;
        private readonly static float moveToJumpSecond = 0.165f;

        private MoveStatus moveStatus;
        
        public PlayerIdleAndMoveState(PlayerController playerController) : base(playerController)
        {
        }

        public override void InStatus(PlayerState previousState, PlayerStateData receiveData)
        {
            playerController.ResetInputVectorFromCamera();
            isMoveToJump = false;
            moveToJumpTime = 0;
        }

        public override PlayerState FixedUpdate()
        {
            if (isMoveToJump) return PlayerState.None;
            moveStatus = playerController.Move();
            if (moveStatus == MoveStatus.NoMove)
            {
                playerController.AnimationController.PlayIdle();
            }
            else
            {
                playerController.AnimationController.PlayMove(moveStatus);
            }
            return PlayerState.None;
        }

        public override PlayerState Update()
        {
            // ジャンプ準備が終わったら、ジャンプに遷移
            if (isMoveToJump)
            {
                moveToJumpTime += Time.deltaTime;
                if (moveToJumpTime >= moveToJumpSecond)
                {
                    playerController.JumpStart(playerController.PlayerSettings.GetMoveSpeed(moveStatus), NextStateData.forward);
                    return PlayerState.InAir;
                }
                return PlayerState.None;
            }
            
            if (playerController.IsInputJump)
            {
                isMoveToJump = true;
                NextStateData.forward = playerController.JumpReady();
                playerController.AnimationController.PlayJump();
                return PlayerState.None;
            }

            if (!playerController.IsOnGround)
            {
                playerController.AnimationController.PlayInAir();
                NextStateData.forward = playerController.GetPlayerForward();
                return PlayerState.InAir;
            }

            // ダメージ受け
            if (playerController.IsDamaged)
            {
                return PlayerState.Damaged;
            }

            // 回避
            if (playerController.IsInputDodge)
            {
                NextStateData.forward = playerController.GetInputForward();
                return PlayerState.Dodge;
            }

            // 攻撃
            if (playerController.IsInputAttack)
            {
                NextStateData.forward = playerController.GetInputForward();
                return PlayerState.Attack; 
            }

            return PlayerState.None;
        }
    }
}
