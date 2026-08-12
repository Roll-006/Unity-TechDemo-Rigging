using Rigging.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Player
{
    public class PlayerMovement : MonoBehaviour
    {
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
            if (animator == null)
                throw new InvalidOperationException("Animator の取得に失敗しました。SerializeField に登録してください。");

            if (rb == null)
                throw new InvalidOperationException("Rigidbody の取得に失敗しました。SerializeField に登録してください。");
        }

        private void Update()
        {
            rb.angularVelocity = Vector3.zero;

            UpdateMoveDirection();
            UpdateAnimatorParameters();
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

        public void StopSprint()
        {
            IsSprinting = false;
        }

        private void OnMove(InputValue value)
        {
            _inputAxis = value.Get<Vector2>();
        }

        private void OnSprint(InputValue value)
        {
            IsSprinting = !IsSprinting;
        }
    }
}
