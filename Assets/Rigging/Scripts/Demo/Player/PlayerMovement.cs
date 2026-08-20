using Rigging.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Demo.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference sprintAction;
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float towardSpeed;

        /// <summary>
        /// カメラ基準の移動方向
        /// </summary>
        public Vector3 MoveDirection { get; private set; }
        public bool IsMoving => MoveDirection.sqrMagnitude > Mathf.Epsilon;
        public bool IsSprinting { get; private set; }

        private Vector2 _inputAxis;
        private Vector3 _localMoveDirection;

        private void Awake()
        {
            if (playerInput == null)
                throw new InvalidOperationException($"{nameof(playerInput)} ({typeof(PlayerInput).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (moveAction == null)
                throw new InvalidOperationException($"{nameof(moveAction)} ({typeof(InputActionReference).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (sprintAction == null)
                throw new InvalidOperationException($"{nameof(sprintAction)} ({typeof(InputActionReference).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (animator == null)
                throw new InvalidOperationException($"{nameof(animator)} ({typeof(Animator).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (rb == null)
                throw new InvalidOperationException($"{nameof(rb)} ({typeof(Rigidbody).Name}) の取得に失敗しました。SerializeField に登録してください。");
        }

        private void OnEnable()
        {
            playerInput.onActionTriggered += OnActionTriggered;
        }

        private void OnDisable()
        {
            playerInput.onActionTriggered -= OnActionTriggered;
        }

        private void Update()
        {
            rb.angularVelocity = Vector3.zero;
            UpdateMoveDirection();
        }

        private void LateUpdate()
        {
            UpdateAnimatorParameters();
        }

        private void OnActionTriggered(InputAction.CallbackContext context)
        {
            if (OnMove(context)) return;
            if (OnSprint(context)) return;
        }

        private bool OnMove(InputAction.CallbackContext context)
        {
            if (context.action != moveAction.action) return false;

            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    _inputAxis = context.ReadValue<Vector2>();
                    break;

                case InputActionPhase.Canceled:
                    _inputAxis = Vector2.zero;
                    break;
            }

            return true;
        }

        private bool OnSprint(InputAction.CallbackContext context)
        {
            if (context.action != sprintAction.action) return false;

            switch (context.phase)
            {
                case InputActionPhase.Started:
                    IsSprinting = !IsSprinting;
                    break;
            }

            return true;
        }

        public void StopSprint()
        {
            IsSprinting = false;
        }

        private void UpdateMoveDirection()
        {
            var cameraTransform = CameraManager.I.OutputCamera.transform;

            var cameraForward = cameraTransform.forward;
            var cameraRight = cameraTransform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // 入力をカメラ基準のベクトルに変換
            var targetMoveDirection = cameraRight * _inputAxis.x + cameraForward * _inputAxis.y;
            targetMoveDirection = Vector3.ClampMagnitude(targetMoveDirection, 1f);

            MoveDirection = Vector3.MoveTowards(MoveDirection, targetMoveDirection, towardSpeed * Time.deltaTime);
            
            _localMoveDirection = transform.InverseTransformDirection(MoveDirection);
        }

        private void UpdateAnimatorParameters()
        {
            animator.SetBool("IsMoving", IsMoving);
            animator.SetBool("IsSprinting", IsSprinting);
            animator.SetFloat("PlayerMoveDirectionX", _localMoveDirection.x);
            animator.SetFloat("PlayerMoveDirectionZ", _localMoveDirection.z);
        }
    }
}
