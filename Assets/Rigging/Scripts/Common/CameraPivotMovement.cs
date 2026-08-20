using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Common
{
    public class CameraPivotMovement : MonoBehaviour
    {
        [SerializeField] private InputActionReference actionReference;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField, Range(-180f, 180f)] private float maxYaw;
        [SerializeField, Range(-180f, 180f)] private float minYaw;
        [SerializeField, Range(-90f, 90f)] private float maxPitch;
        [SerializeField, Range(-90f, 90f)] private float minPitch;
        [SerializeField] private bool isWrapYaw;
        [SerializeField] private bool isWrapPitch;
        [SerializeField] private bool isInvertYaw;
        [SerializeField] private bool isInvertPitch;
        [SerializeField] private float speed;

        public bool IsMoving => _axis.sqrMagnitude > Mathf.Epsilon;
        public Vector2 Input { get; private set; }

        private Vector2 _axis;

        private void Awake()
        {
            if (actionReference == null)
                throw new InvalidOperationException($"{nameof(actionReference)} ({typeof(InputActionReference).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (cinemachineCamera == null)
                throw new InvalidOperationException($"{nameof(cinemachineCamera)} ({typeof(CinemachineCamera).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (minYaw >= maxYaw)
                throw new ArgumentOutOfRangeException($"{nameof(minYaw)}, {nameof(maxYaw)} の範囲制限が不正な値です。有効範囲は [ {nameof(minYaw)} < {nameof(maxYaw)} ] です。");

            if (minPitch >= maxPitch)
                throw new ArgumentOutOfRangeException($"{nameof(minPitch)}, {nameof(maxPitch)} の範囲制限が不正な値です。有効範囲は [ {nameof(minPitch)} < {nameof(maxPitch)} ] です。");
        }

        private void LateUpdate()
        {
            Rotate();
        }

        private void Rotate()
        {
            AddInput();
            ApplyLimits();

            cinemachineCamera.Target.TrackingTarget.transform.rotation = Quaternion.Euler(_axis.y, _axis.x, 0f);
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
