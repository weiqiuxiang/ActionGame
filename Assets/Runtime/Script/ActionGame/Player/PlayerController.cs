using System;
using System.Collections;
using System.Collections.Generic;
using Project.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

namespace Project.ActionGame
{
    /// <summary>
    /// プレイヤーを操作
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("参照")] 
        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private CapsuleCollider playerCollider;

        [Header("パラメータ")] 
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float fallAcc;
        [SerializeField] private float fallSpeedMax;
        [SerializeField] private float maxSlopeAngle;
        [SerializeField] private float stepHeight;
        [SerializeField] private float stepDistance;
        [SerializeField] private float rotatePercentSecond;

        private readonly float groundRayDistance = 0.1f;

        private InputController inputController;
        
        private Vector2 inputVector = Vector2.zero;  // 入力ベクトル
        private Vector3 moveVector = Vector3.zero;
        private float moveSpeed = 0;
        private bool HasNoInput => Mathf.Approximately(moveSpeed, 0);
        private float fallSpeed = 0;
        
        public bool IsInputDash { get; private set; } = false;   // ダッシュ入力しているか
        public bool IsInputDodge { get; private set; } = false; // 回避を入力したか
        public bool IsInputJump { get; private set; } = false;   // ジャンプを入力したか
        public bool IsInputAttack { get; private set; } = false;   // 攻撃を入力したか
        public bool IsOnGround { get; private set; } = true;    // 地面と接触しているかどうか
        public bool IsDamaged { get; private set; } = false;    // ダメージを受けたか？
        public bool IsCanFall { get; set; } = true; // 自然落下発生するかどうか

        private void Start()
        {
            Initalize();
        }

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
            IsInputDash = false;
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
            inputVector = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// 移動処理
        /// </summary>
        public void Move()
        {
            float inputValue = inputVector.magnitude;
            if (inputValue < playerSettings.MoveInputDeadZone)
            {
                moveSpeed = 0;
            }
            else
            {
                moveSpeed = IsInputDash ? playerSettings.DashSpeed :
                    (inputValue < playerSettings.WalkInputThreshold) ? playerSettings.WalkSpeed : playerSettings.RunSpeed;
            }

            MoveCharacter();
            RotateCharacter();
        }

        /// <summary>
        /// 空中の移動
        /// </summary>
        public void AirMove()
        {
            float inputValue = inputVector.magnitude;
            if (inputValue < playerSettings.MoveInputDeadZone)
            {
                moveSpeed = 0;
            }
            else
            {
                moveSpeed = IsInputDash ? playerSettings.DashSpeed :
                    (inputValue < playerSettings.WalkInputThreshold) ? playerSettings.WalkSpeed : playerSettings.RunSpeed;
            }

            MoveCharacter();
        }
        
        private void MoveCharacter()
        {
            if (!HasNoInput)
            {
                moveVector = cameraTransform.forward * inputVector.y + cameraTransform.right * inputVector.x;
                moveVector.y = 0;
                moveVector = moveVector.normalized;
            }

            rigidbody.velocity = new Vector3(moveSpeed * moveVector.x, fallSpeed, moveSpeed * moveVector.z);
        }

        private void RotateCharacter()
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVector, Vector3.up);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, rotatePercentSecond * Time.deltaTime);
        }

        public void SetMoveDirectionEqualPlayerDirection()
        {
            moveVector = playerTransform.forward;
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
                IsInputDash = true;
            }

            if (context.canceled)
            {
                if (!IsInputDash)
                {
                    // 走り入力していない場合は回避になる
                    IsInputDodge = true;
                }
                IsInputDash = false;
            }
        }

        private void FixedUpdate()
        {
            RaycastHit groundHit = Fall();
            if (IsOnGround && !HasNoInput)
            {
                //CheckStep();
            }
        }

        private void Update()
        {
            //Debug.LogError(rigidbody.velocity);
        }

        /// <summary>
        /// 落下
        /// </summary>
        private RaycastHit Fall()
        {
            if (!IsCanFall) return default;

            var bounds = playerCollider.bounds;
            float rayDistance = bounds.center.y - bounds.min.y + groundRayDistance;
            IsOnGround = Physics.Raycast(bounds.center, Vector3.down, out RaycastHit hit, rayDistance, groundLayer);
            
            if (!IsOnGround)
            {
                AddFallSpeed();
                return hit;
            }

            // 地面上にいるが、浮いている状態を解消
            if (bounds.min.y - hit.point.y > 0.1f)
            {
                rigidbody.position = rigidbody.position.AddY(-0.01f * Time.deltaTime);
            }

            fallSpeed = 0;

            return hit;
        }

        private void AddFallSpeed()
        {
            fallSpeed -= fallAcc * Time.deltaTime;
            fallSpeed = Mathf.Max(fallSpeed, -fallSpeedMax);
        }

        private void CheckStep()
        {
            Vector3 stepOffset = moveVector * moveSpeed * Time.deltaTime * stepDistance;
            
            
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var bounds = playerCollider.bounds;
            Vector3 lineEnd = new Vector3(bounds.center.x, bounds.min.y + groundRayDistance, bounds.center.z);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(bounds.center, lineEnd);
        }
#endif
    }
}
