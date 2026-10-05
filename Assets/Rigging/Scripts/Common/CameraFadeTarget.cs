using Rigging.Core;
using UnityEngine;

namespace Rigging.Common
{
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
            var cameraPosition = CameraManager.I.OutputCamera.transform.position;
            renderer.enabled = Vector3.Distance(targetTransform.position, cameraPosition) > transparentDistance;
        }
    }
}
