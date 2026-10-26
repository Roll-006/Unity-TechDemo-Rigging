using Rigging.Core;
using UnityEngine;

namespace Rigging.Common
{
    /// <summary>
    /// カメラが接近した際に描画を停止する。
    /// </summary>
    public class CameraFadeTarget : MonoBehaviour
    {
        [SerializeField] private new Renderer renderer;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private float transparentDistance;

        private void Awake()
        {
            Debugger.ValidateSerializeField(renderer, this);
            Debugger.ValidateSerializeField(targetTransform, this);
        }

        private void Update()
        {
            Transparent();
        }

        private void Transparent()
        {
            var outputCamera = CameraManager.I.OutputCamera;
            if (outputCamera == null) return;
            
            var cameraPosition = outputCamera.transform.position;
            renderer.enabled = Vector3.Distance(targetTransform.position, cameraPosition) > transparentDistance;
        }
    }
}