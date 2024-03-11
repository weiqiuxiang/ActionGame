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
        [SerializeField] private ActionGameEnvironmentSetting environmentSetting;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private CapsuleCollider playerCollider;

        [Header("パラメータ")] 
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float maxSlopeAngle;
        [SerializeField] private float stepHeight;
        [SerializeField] private float stepDistance;
        [SerializeField] private float rotatePercentSecond;

        private readonly float groundRayDistance = 0.1f;

        private InputController inputController;
        
        private Vector2 inputVector = Vector2.zero;  // 入力ベクトル
        private Vector3 inputVectorFromCamera = Vector3.zero;  // カメラに対する入力方向
        private float moveSpeed = 0;
        private bool HasNoInput => Mathf.Approximately(inputVector.x, 0) && Mathf.Approximately(inputVector.y, 0);
        private float fallSpeed = 0;
        
        public bool IsInputDash { get; private set; } = false;   // ダッシュ入力しているか
        public bool IsInputDodge { get; private set; } = false; // 回避を入力したか
        public bool IsInputJump { get; private set; } = false;   // ジャンプを入力したか
        public bool IsInputAttack { get; private set; } = false;   // 攻撃を入力したか
        public bool IsOnGround { get; private set; } = true;    // 地面と接触しているかどうか
        public bool IsDamaged { get; private set; } = false;    // ダメージを受けたか？
        public bool IsCanFall { get; set; } = true; // 自然落下発生するかどうか
        private PlayerStateMachine playerStateMachine;

        /// <summary>
        /// ジャンプ後、しばらくのフレームで空中扱い
        /// </summary>
        private static readonly int NoCheckOnGroundFrame = 5;
        private int noCheckOnGroundCount = 0;    

        private void Start()
        {
            Initalize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initalize()
        {
            if (playerStateMachine == null)
            {
                playerStateMachine = new PlayerStateMachine(this);
            }
            playerStateMachine.Initialize();
            
            InitializeFlags();
            InitializeInput();
            RegisterInput();
        }

        private void InitializeFlags()
        {
            IsInputDash = false;
            IsInputDodge = false;
            IsInputJump = false;
            IsInputAttack = false;
            IsDamaged = false;
            IsCanFall = true;
        }

        private void ResetInputValuesAfterStateMachineUpdate()
        {
            IsInputDodge = false;
            IsInputJump = false;
            IsInputAttack = false;
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
            inputController.Player.Move.started += InputMove;
            inputController.Player.Move.performed += InputMove;
            inputController.Player.Move.canceled += InputMove;

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
        private void InputMove(InputAction.CallbackContext context)
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
            
            if (!HasNoInput)
            {
                CalcInputVectorFromCamera();
            }
            rigidbody.velocity = new Vector3(moveSpeed * inputVectorFromCamera.x, fallSpeed, moveSpeed * inputVectorFromCamera.z);
            
            RotateCharacter();
        }

        /// <summary>
        /// 空中の移動
        /// </summary>
        public void AirMove()
        {
            rigidbody.velocity = rigidbody.velocity.SetY(fallSpeed);

            // 移動方向の速度を徐々減少
            Vector3 reserve = -rigidbody.velocity.SetY(0).normalized;
            rigidbody.velocity += Time.deltaTime * reserve * 0.1f * playerSettings.AirMoveAcc;
            
            if (HasNoInput) return;

            // 空中入力で移動
            CalcInputVectorFromCamera();
            rigidbody.velocity += Time.deltaTime * new Vector3(inputVectorFromCamera.x * playerSettings.AirMoveAcc, 0, inputVectorFromCamera.z * playerSettings.AirMoveAcc);
        }

        private void CalcInputVectorFromCamera()
        {
            inputVectorFromCamera = cameraTransform.forward * inputVector.y + cameraTransform.right * inputVector.x;
            inputVectorFromCamera.y = 0;
            inputVectorFromCamera = inputVectorFromCamera.normalized;
        }

        public void MoveToFall()
        {
            rigidbody.velocity = rigidbody.velocity.SetXZ(inputVectorFromCamera.x * playerSettings.FallStartSpeed, inputVectorFromCamera.z * playerSettings.FallStartSpeed);
        }

        public void Jump()
        {
            fallSpeed += Mathf.Sqrt(playerSettings.JumpHeight * environmentSetting.Gravity * 2);
            IsOnGround = false;
            noCheckOnGroundCount = NoCheckOnGroundFrame;
        }

        private void RotateCharacter()
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputVectorFromCamera, Vector3.up);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, rotatePercentSecond * Time.deltaTime);
        }

        public void ResetInputVectorFromCamera()
        {
            inputVectorFromCamera = playerTransform.forward;
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
            RaycastHit groundHit = CheckIsOnGround();
            Fall();
            
            playerStateMachine.FixedUpdateState();
        }

        private void Update()
        {
            playerStateMachine.UpdateState();

            // 各入力をリセット
            ResetInputValuesAfterStateMachineUpdate();
        }

        /// <summary>
        /// 地面にいるかどうかチェック
        /// </summary>
        private RaycastHit CheckIsOnGround()
        {
            if (noCheckOnGroundCount > 0)
            {
                noCheckOnGroundCount--;
                return default;
            }
            
            var bounds = playerCollider.bounds;
            float rayDistance = bounds.center.y - bounds.min.y + groundRayDistance;
            IsOnGround = Physics.Raycast(bounds.center, Vector3.down, out RaycastHit hit, rayDistance, groundLayer);
            
            // 地面上にいるが、浮いている状態を解消
            if (bounds.min.y - hit.point.y > 0.1f)
            {
                rigidbody.position = rigidbody.position.AddY(-0.01f * Time.deltaTime);
            }

            return hit;
        }
        
        /// <summary>
        /// 落下
        /// </summary>
        private void Fall()
        {
            if (!IsCanFall) return;
            
            if (!IsOnGround)
            {
                AddFallSpeed();
                return;
            }
            
            // 地面時落下速度を0に
            fallSpeed = 0;
        }

        private void AddFallSpeed()
        {
            fallSpeed -= environmentSetting.Gravity * Time.deltaTime;
            fallSpeed = Mathf.Max(fallSpeed, -environmentSetting.FallSpeedMax);
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
