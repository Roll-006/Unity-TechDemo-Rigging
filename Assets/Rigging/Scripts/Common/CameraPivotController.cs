using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Common
{
    public class CameraPivotController : MonoBehaviour
    {
        [SerializeField] private InputActionReference actionReference;
        [SerializeField] private Transform cameraPivot;
        [SerializeField, Range(-180f, 180f)] private float maxYaw;
        [SerializeField, Range(-180f, 180f)] private float minYaw;
        [SerializeField, Range(-90f, 90f)] private float maxPitch;
        [SerializeField, Range(-90f, 90f)] private float minPitch;
        [SerializeField] private bool isWrapYaw;
        [SerializeField] private bool isWrapPitch;
        [SerializeField] private bool isInvertYaw;
        [SerializeField] private bool isInvertPitch;
        [SerializeField] private float speed;

        private Vector2 _axis;

        private void Awake()
        {
            if (actionReference == null)
                throw new InvalidOperationException("InputActionReference の取得に失敗しました。SerializeField に登録してください。");

            if (cameraPivot == null)
                throw new InvalidOperationException("Camera Pivot の取得に失敗しました。SerializeField に登録してください。");

            if (minYaw >= maxYaw)
                throw new ArgumentOutOfRangeException("Yaw の範囲制限が不正な値です。有効範囲は [ Min Yaw < Max Yaw ] です。");

            if (minPitch >= maxPitch)
                throw new ArgumentOutOfRangeException("Pitch の範囲制限が不正な値です。有効範囲は [ Min Pitch < Max Pitch ] です。");
        }

        private void LateUpdate()
        {
            Rotate();
        }

        private void Rotate()
        {
            AddInput();
            ApplyLimits();

            cameraPivot.rotation = Quaternion.Euler(_axis.y, _axis.x, 0f);
        }

        private void AddInput()
        {
            var input = actionReference.action.ReadValue<Vector2>();
            input.Scale(new Vector2(isInvertYaw ? -1 : 1, isInvertPitch ? -1 : 1));
            _axis += input * speed;
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
