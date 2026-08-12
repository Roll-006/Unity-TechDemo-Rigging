using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Rigging.Common
{
    public class CameraOrbitMovement : MonoBehaviour
    {
        [SerializeField] private float topCameraDistance;
        [SerializeField] private float bottomCameraDistance;
        [SerializeField] private float topShoulderOffsetZ;
        [SerializeField] private float bottomShoulderOffsetZ;
        [SerializeField] private CinemachineThirdPersonFollow thirdPersonFollow;
        [SerializeField] private Transform cameraPivot;
        [SerializeField, Range(-90f, 90f)] private float maxPitch;
        [SerializeField, Range(-90f, 90f)] private float minPitch;

        private float _defaultCameraDistance;
        private float _defaultShoulderOffsetZ;

        private void Awake()
        {
            if (thirdPersonFollow == null)
                throw new InvalidOperationException("CinemachineThirdPersonFollow の取得に失敗しました。SerializeField に登録してください。");

            if (cameraPivot == null)
                throw new InvalidOperationException("Camera Pivot の取得に失敗しました。SerializeField に登録してください。");

            if (minPitch >= maxPitch)
                throw new ArgumentOutOfRangeException("Pitch の範囲制限が不正な値です。有効範囲は [ Min Pitch < Max Pitch ] です。");

            _defaultCameraDistance = thirdPersonFollow.CameraDistance;
            _defaultShoulderOffsetZ = thirdPersonFollow.ShoulderOffset.z;
        }

        public void LateUpdate()
        {
            UpdateCameraDistance();
        }

        private void UpdateCameraDistance()
        {
            var pitchDegree = Mathf.DeltaAngle(0, cameraPivot.eulerAngles.x);
            var t = Mathf.InverseLerp(minPitch, maxPitch, pitchDegree);

            // ベジェ曲線で補間
            thirdPersonFollow.CameraDistance = MathUtility.QuadraticBezierCurve(bottomCameraDistance, _defaultCameraDistance, topCameraDistance, t);
            thirdPersonFollow.ShoulderOffset.z = MathUtility.QuadraticBezierCurve(bottomShoulderOffsetZ, _defaultShoulderOffsetZ, topShoulderOffsetZ, t);
        }
    }
}
