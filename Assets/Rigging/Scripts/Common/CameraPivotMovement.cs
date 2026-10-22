using Rigging.Core;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Common
{
    public class CameraPivotMovement : MonoBehaviour
    {
        [SerializeField] private InputActionReference actionReference;
        [SerializeField] private CinemachineCamera cinemachine;
        [SerializeField, Range(-180f, 180f)] private float maxYaw;
        [SerializeField, Range(-180f, 180f)] private float minYaw;
        [SerializeField, Range(-90f, 90f)] private float maxPitch;
        [SerializeField, Range(-90f, 90f)] private float minPitch;
        [SerializeField] private bool isWrapYaw;
        [SerializeField] private bool isWrapPitch;
        [SerializeField] private bool isInvertYaw;
        [SerializeField] private bool isInvertPitch;
        [SerializeField, Min(0f)] private float speedMagnitude;
        [SerializeField, Min(0f)] private float maxSpeed;

        public bool IsMoving => Input.sqrMagnitude > Mathf.Epsilon;
        public float InputMagnitude => Input.magnitude;
        public Vector2 Input { get; private set; }

        private Vector2 _axis;

        private void Awake()
        {
            Debugger.ValidateSerializeField(actionReference, this);
            Debugger.ValidateSerializeField(cinemachine, this);
            Debugger.ValidateRange(minYaw, maxYaw);
            Debugger.ValidateRange(minPitch, maxPitch);
        }

        private void LateUpdate()
        {
            Rotate();
        }

        private void Rotate()
        {
            AddInput();
            ApplyLimits();

            cinemachine.Target.TrackingTarget.transform.rotation = Quaternion.Euler(_axis.y, _axis.x, 0f);
        }

        private void AddInput()
        {
            Input = actionReference.action.ReadValue<Vector2>();
            Input *= new Vector2(isInvertYaw ? -1 : 1, isInvertPitch ? -1 : 1) * speedMagnitude;
            if (Input.magnitude > maxSpeed)
            {
                Input = Vector3.ClampMagnitude(Input, maxSpeed);
            }
            
            _axis += Input;
        }

        private void ApplyLimits()
        {
            _axis.x = isWrapYaw ? Wrap(_axis.x, minYaw, maxYaw) : Mathf.Clamp(_axis.x, minYaw, maxYaw);
            _axis.y = isWrapPitch ? Wrap(_axis.y, minPitch, maxPitch) : Mathf.Clamp(_axis.y, minPitch, maxPitch);
        }

        private float Wrap(float value, float min, float max)
        {
            var range = max - min;
            return min + Mathf.Repeat(value - min, range);
        }
    }
}