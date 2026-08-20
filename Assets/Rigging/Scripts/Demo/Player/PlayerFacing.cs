using Rigging.Core;
using System;
using UnityEngine;

namespace Rigging.Demo.Player
{
    public class PlayerFacing : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private float towardSpeed;

        public bool IsSyncCameraForward { get; private set; }

        private Vector3 _currentFacingDirection;

        private void Awake()
        {
            if (playerMovement == null)
                throw new InvalidOperationException($"{nameof(playerMovement)} ({typeof(PlayerMovement).Name}) の取得に失敗しました。SerializeField に登録してください。");
        }

        private void Start()
        {
            _currentFacingDirection = transform.forward;
        }

        private void Update()
        {
            IsSyncCameraForward = false;
        }

        private void LateUpdate()
        {
            UpdateFacingDirection();
        }

        public void SyncCameraForward()
        {
            IsSyncCameraForward = true;
        }

        private void UpdateFacingDirection()
        {
            var targetFacingDirection = IsSyncCameraForward ? GetMoveCameraForward() : playerMovement.MoveDirection;

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
