using Rigging.Core;
using Rigging.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Demo.Characters.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference sprintAction;
        [SerializeField] private InputActionReference crouchAction;
        [SerializeField] private Animator animator;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private float towardSpeed;

        /// <summary>
        /// カメラ基準の移動方向
        /// </summary>
        public Vector3 MoveDirection { get; private set; }
        public bool IsMoving => MoveDirection.sqrMagnitude > Mathf.Epsilon;
        public bool IsSprinting { get; private set; }
        public bool IsCrouching { get; private set; }

        private Vector2 _inputAxis;
        private Vector3 _localMoveDirection;

        private void Awake()
        {
            Debugger.ValidateSerializeField(playerInput, this);
            Debugger.ValidateSerializeField(moveAction, this);
            Debugger.ValidateSerializeField(sprintAction, this);
            Debugger.ValidateSerializeField(crouchAction, this);
            Debugger.ValidateSerializeField(animator, this);
            Debugger.ValidateSerializeField(rigidbody, this);
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
            rigidbody.angularVelocity = Vector3.zero;
            UpdateMoveDirection();
            UpdateAnimatorParameters();
        }

        private void OnActionTriggered(InputAction.CallbackContext context)
        {
            if (OnMove(context)) return;
            if (OnSprint(context)) return;
            if (OnCrouch(context)) return;
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
                    IsCrouching = false;
                    break;
            }

            return true;
        }

        private bool OnCrouch(InputAction.CallbackContext context)
        {
            if (context.action != crouchAction.action) return false;

            switch (context.phase)
            {
                case InputActionPhase.Started:
                    IsCrouching = !IsCrouching;
                    IsSprinting = false;
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
            var cameraTransform = CameraManager.I.ActiveCinemachine.transform;

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
            animator.SetBool(AnimatorParameters.IsMoving, IsMoving);
            animator.SetBool(AnimatorParameters.IsSprinting, IsSprinting);
            animator.SetBool(AnimatorParameters.IsCrouching, IsCrouching);
            animator.SetFloat(AnimatorParameters.PlayerMoveDirectionX, _localMoveDirection.x);
            animator.SetFloat(AnimatorParameters.PlayerMoveDirectionZ, _localMoveDirection.z);
        }
    }
}