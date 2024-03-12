using UnityEngine;

namespace Project.ActionGame
{
    /// <summary>
    /// 地面にいる時の動き、移動、ダッシュなどをコントロール
    /// </summary>
    public class PlayerIdleAndMoveState : PlayerStatusBase
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

        public override void InStatus(PlayerStatus lastStatus)
        {
            playerController.ResetInputVectorFromCamera();
            isMoveToJump = false;
            moveToJumpTime = 0;
        }

        public override void OutStatus()
        {
            
        }

        public override PlayerStatus FixedUpdate()
        {
            if (isMoveToJump) return PlayerStatus.None;
            moveStatus = playerController.Move();
            if (moveStatus == MoveStatus.NoMove)
            {
                playerController.AnimationController.PlayIdle();
            }
            else
            {
                playerController.AnimationController.PlayMove(moveStatus);
            }
            return PlayerStatus.None;
        }

        public override PlayerStatus Update()
        {
            // ジャンプ準備が終わったら、ジャンプに遷移
            if (isMoveToJump)
            {
                moveToJumpTime += Time.deltaTime;
                if (moveToJumpTime >= moveToJumpSecond)
                {
                    playerController.Jump(playerController.PlayerSettings.GetMoveSpeed(moveStatus));
                    return PlayerStatus.InAir;
                }
                return PlayerStatus.None;
            }
            
            if (playerController.IsInputJump)
            {
                isMoveToJump = true;
                playerController.JumpReady();
                playerController.AnimationController.PlayJump();
                return PlayerStatus.None;
            }

            if (!playerController.IsOnGround)
            {
                playerController.AnimationController.PlayInAir();
                playerController.ResetAirForward();
                return PlayerStatus.InAir;
            }

            // ダメージ受け
            if (playerController.IsDamaged)
            {
                return PlayerStatus.Damaged;
            }

            // 回避
            if (playerController.IsInputDodge)
            {
                return PlayerStatus.Dodge;
            }

            // 攻撃
            if (playerController.IsInputAttack)
            {
                //return PlayerStatus.Attack; 
            }

            return PlayerStatus.None;
        }
    }
}
