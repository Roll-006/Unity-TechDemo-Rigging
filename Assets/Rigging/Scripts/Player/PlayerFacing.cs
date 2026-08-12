using Rigging.Core;
using System;
using UnityEngine;

namespace Rigging.Player
{
    public class PlayerFacing : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private float towardSpeed;

        private Vector3 _currentFacingDirection;
        private bool _isSyncCameraForward;

        private void Awake()
        {
            if (playerMovement == null)
                throw new InvalidOperationException("PlayerMovement ÇÃéÊìæÇ…é∏îsÇµÇ‹ÇµÇΩÅBSerializeField Ç…ìoò^ÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB");
        }

        private void Start()
        {
            _currentFacingDirection = transform.forward;
        }

        private void Update()
        {
            UpdateSyncState();
            UpdateFacingDirection();
        }

        private void UpdateSyncState()
        {
            if (playerMovement.IsSprinting)
            {
                _isSyncCameraForward = false;
                return;
            }

            _isSyncCameraForward = true;
        }

        private void UpdateFacingDirection()
        {
            if (!playerMovement.IsMoving) return;

            var targetFacingDirection = _isSyncCameraForward ? GetMoveCameraForward() : playerMovement.MoveDirection;

            _currentFacingDirection = Vector3.RotateTowards(_currentFacingDirection, targetFacingDirection, towardSpeed * Time.deltaTime, 0f);
            transform.LookAt(transform.position + _currentFacingDirection);
        }

        private Vector3 GetMoveCameraForward()
        {
            var forward = CameraManager.I.OutputCamera.transform.forward;
            forward.y = 0f;
            return forward.normalized;
        }
    }
}
