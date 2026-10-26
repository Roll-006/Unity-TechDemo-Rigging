using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Characters.Player
{
    public class PlayerFacing : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private float towardSpeed;

        public bool IsSyncCameraForward { get; private set; }

        private Vector3 _currentFacingDirection;

        private void Awake()
        {
            Debugger.ValidateSerializeField(playerMovement, this);
            Debugger.ValidateSerializeField(rigidbody, this);
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
            if (targetFacingDirection.sqrMagnitude <= Mathf.Epsilon) return;

            _currentFacingDirection = Vector3.RotateTowards(_currentFacingDirection, targetFacingDirection, towardSpeed * Time.deltaTime, 0f);
            rigidbody.MoveRotation(Quaternion.LookRotation(_currentFacingDirection, Vector3.up));
        }

        private Vector3 GetMoveCameraForward()
        {
            var forward = CameraManager.I.ActiveCinemachine.transform.forward;
            forward.y = 0f;
            return forward.normalized;
        }
    }
}