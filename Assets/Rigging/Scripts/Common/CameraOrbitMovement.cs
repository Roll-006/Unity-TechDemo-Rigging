using Rigging.Core;
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
        [SerializeField] private CinemachineCamera cinemachine;
        [SerializeField, Range(-90f, 90f)] private float maxPitch;
        [SerializeField, Range(-90f, 90f)] private float minPitch;

        private float _defaultCameraDistance;
        private float _defaultShoulderOffsetZ;

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(thirdPersonFollow, this);
            Debugger.I.ValidateSerializeField(cinemachine, this);
            Debugger.I.ValidateRange(minPitch, maxPitch);

            _defaultCameraDistance = thirdPersonFollow.CameraDistance;
            _defaultShoulderOffsetZ = thirdPersonFollow.ShoulderOffset.z;
        }

        public void LateUpdate()
        {
            UpdateCameraDistance();
        }

        private void UpdateCameraDistance()
        {
            var pitchDegree = Mathf.DeltaAngle(0, cinemachine.Target.TrackingTarget.eulerAngles.x);
            var t = Mathf.InverseLerp(minPitch, maxPitch, pitchDegree);

            // ベジェ曲線で補間
            thirdPersonFollow.CameraDistance = MathUtility.QuadraticBezierCurve(bottomCameraDistance, _defaultCameraDistance, topCameraDistance, t);
            thirdPersonFollow.ShoulderOffset.z = MathUtility.QuadraticBezierCurve(bottomShoulderOffsetZ, _defaultShoulderOffsetZ, topShoulderOffsetZ, t);
        }
    }
}
