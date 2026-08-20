using Rigging.Core;
using System;
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
            if (renderer == null)
                throw new InvalidOperationException($"{nameof(renderer)} ({typeof(Renderer).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (targetTransform == null)
                throw new InvalidOperationException($"{nameof(targetTransform)} ({typeof(Transform).Name}) の取得に失敗しました。SerializeField に登録してください。");
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
