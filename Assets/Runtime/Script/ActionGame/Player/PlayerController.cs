using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.ActionGame
{
    /// <summary>
    /// プレイヤーを操作
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("参照")] 
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Rigidbody playerRigidBody;
        [SerializeField] private Animator playerAnimator;
        public Animator PlayerAnimator => playerAnimator;
        
        [Header("パラメータ")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float runSpeed;

        private InputController inputController;
        
        private Vector2 moveVector = Vector2.zero;  // 移動ベクトル
        public bool IsInputRun { get; private set; } = false;   // 走る入力しているか
        public bool IsInputDodge { get; private set; } = false; // 回避を入力したか
        public bool IsInputJump { get; private set; } = false;   // ジャンプを入力したか
        public bool IsInputAttack { get; private set; } = false;   // 攻撃を入力したか
        public bool IsOnGround { get; private set; } = true;    // 地面と接触しているかどうか
        public bool IsDamaged { get; private set; } = false;    // ダメージを受けたか？
        public bool IsCanFall { get; set; } // 落下発生するかどうか

        private readonly float MoveInputDeadZone = 0.01f;
        
        /// <summary>
        /// 初期化
        /// </summary>
        public void Initalize()
        {
            ResetInputValues();
            InitializeInput();
            RegisterInput();
        }

        public void ResetInputValues()
        {
            moveVector = Vector2.zero;
            IsInputRun = false;
            IsInputDodge = false;
            IsInputJump = false;
            IsInputAttack = false;
            IsDamaged = false;
            IsCanFall = true;
        }

        public void ResetDodgeInput() => IsInputDodge = false;

        private void InitializeInput()
        {
            // すでに入力装置が存在する場合、破棄する
            if (inputController != null)
            {
                inputController.Disable();
            }
            inputController = new InputController();
            inputController.Enable();
        }

        private void RegisterInput()
        {
            inputController.Player.Move.started += Move;
            inputController.Player.Move.performed += Move;
            inputController.Player.Move.canceled += Move;

            inputController.Player.Attack.performed += Attack;
            inputController.Player.Jump.performed += Jump;
            
            inputController.Player.RunOrDodge.performed += RunOrDodge;
            inputController.Player.RunOrDodge.canceled += RunOrDodge;
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public void Pause()
        {
            inputController.Disable();
        }
        
        /// <summary>
        /// 再開
        /// </summary>
        public void Resume()
        {
            inputController.Enable();
        }

        /// <summary>
        /// 移動入力
        /// </summary>
        /// <param name="context"></param>
        private void Move(InputAction.CallbackContext context)
        {
            moveVector = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// 移動処理
        /// </summary>
        public void Move()
        {
            if (moveVector.magnitude < MoveInputDeadZone) return;
            float speed = IsInputRun ? runSpeed : moveSpeed;
            playerRigidBody.MovePosition(playerTransform.position + speed * Time.deltaTime * new Vector3(moveVector.x, 0, moveVector.y));
        }
        
        /// <summary>
        /// 攻撃
        /// </summary>
        /// <param name="context"></param>
        private void Attack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsInputAttack = true;
            }
        }
        
        /// <summary>
        /// ジャンプ
        /// </summary>
        /// <param name="context"></param>
        private void Jump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsInputJump = true;
            }
        }
        
        /// <summary>
        /// 移動か回避
        /// </summary>
        /// <param name="context"></param>
        private void RunOrDodge(InputAction.CallbackContext context)
        {
            // interactionはholdなので、走るボタンを一定時間以上押すと、走る
            if (context.performed)
            {
                IsInputRun = true;
            }

            if (context.canceled)
            {
                if (!IsInputRun)
                {
                    // 走り入力していない場合は回避になる
                    IsInputDodge = true;
                }
                IsInputRun = false;
            }
        }

        public void UpdateFall()
        {
            // 落下させる
            if (!IsCanFall) return;
        }

        private void OnCollisionStay(Collision other)
        {
            
        }

        private void OnCollisionExit(Collision other)
        {
            
        }
    }
}
