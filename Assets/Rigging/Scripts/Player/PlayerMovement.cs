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

        public Vector3 CurrentMoveDirection { get; private set; }
        public bool IsMoving => CurrentMoveDirection.sqrMagnitude > Mathf.Epsilon;

        private Vector2 _inputAxis;
        private Vector3 _targetMoveDirection;
        private Vector3 _localMoveDirection;

        private void Awake()
        {
            if (animator == null)
                throw new InvalidOperationException("Animator ‚Ìæ“¾‚É¸”s‚µ‚Ü‚µ‚½BSerializeField ‚É“o˜^‚µ‚Ä‚­‚¾‚³‚¢B");

            if (rb == null)
                throw new InvalidOperationException("Rigidbody ‚Ìæ“¾‚É¸”s‚µ‚Ü‚µ‚½BSerializeField ‚É“o˜^‚µ‚Ä‚­‚¾‚³‚¢B");
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

            // “ü—Í‚ğƒJƒƒ‰Šî€‚ÌƒxƒNƒgƒ‹‚É•ÏŠ·
            _targetMoveDirection = cameraRight * _inputAxis.x + cameraForward * _inputAxis.y;
            _targetMoveDirection = Vector3.ClampMagnitude(_targetMoveDirection, 1f);

            CurrentMoveDirection = Vector3.MoveTowards(CurrentMoveDirection, _targetMoveDirection, towardSpeed * Time.deltaTime);
            
            _localMoveDirection = transform.InverseTransformDirection(CurrentMoveDirection);
        }

        private void UpdateAnimatorParameters()
        {
            animator.SetBool("IsMoving", IsMoving);
            animator.SetFloat("PlayerMoveDirectionX", _localMoveDirection.x);
            animator.SetFloat("PlayerMoveDirectionZ", _localMoveDirection.z);
        }

        private void OnMove(InputValue value)
        {
            _inputAxis = value.Get<Vector2>();
        }
    }
}
