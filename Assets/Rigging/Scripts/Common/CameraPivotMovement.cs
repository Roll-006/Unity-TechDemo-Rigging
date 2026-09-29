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
        [SerializeField] private float speed;

        public bool IsMoving => Input.sqrMagnitude > Mathf.Epsilon;
        public float InputMagnitude => Input.magnitude;
        public Vector2 Input { get; private set; }

        private Vector2 _axis;

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(actionReference, this);
            Debugger.I.ValidateSerializeField(cinemachine, this);
            Debugger.I.ValidateRange(minYaw, maxYaw);
            Debugger.I.ValidateRange(minPitch, maxPitch);
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
            Input *= new Vector2(isInvertYaw ? -1 : 1, isInvertPitch ? -1 : 1);
            _axis += Input * speed;
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
