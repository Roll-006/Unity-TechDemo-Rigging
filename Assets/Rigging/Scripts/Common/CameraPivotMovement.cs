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
        [SerializeField] private CameraPivotShareAxis shareAxis;
        [SerializeField] private CameraPivotLimitData limitData;
        [SerializeField] private bool isInvertYaw;
        [SerializeField] private bool isInvertPitch;
        [SerializeField, Min(0f)] private float speedMagnitude;
        [SerializeField, Min(0f)] private float maxSpeed;

        public bool IsMoving => Input.sqrMagnitude > Mathf.Epsilon;
        public float InputMagnitude => Input.magnitude;
        public Vector2 Input { get; private set; }

        private void Awake()
        {
            Debugger.ValidateSerializeField(actionReference, this);
            Debugger.ValidateSerializeField(cinemachine, this);
            Debugger.ValidateSerializeField(shareAxis, this);
            Debugger.ValidateRange(limitData.MinYaw, limitData.MaxYaw);
            Debugger.ValidateRange(limitData.MinPitch, limitData.MaxPitch);
        }

        private void LateUpdate()
        {
            Rotate();
        }

        private void Rotate()
        {
            if (cinemachine.enabled)
            {
                AddInput();
                shareAxis.MovePivot(Input);
                shareAxis.ApplyLimit(limitData);
            }
            
            cinemachine.Target.TrackingTarget.transform.rotation = Quaternion.Euler(shareAxis.Axis.y, shareAxis.Axis.x, 0f);
        }

        private void AddInput()
        {
            Input = actionReference.action.ReadValue<Vector2>();
            Input *= new Vector2(isInvertYaw ? -1 : 1, isInvertPitch ? -1 : 1) * speedMagnitude;
            if (Input.magnitude > maxSpeed)
            {
                Input = Vector3.ClampMagnitude(Input, maxSpeed);
            }
        }
    }
}