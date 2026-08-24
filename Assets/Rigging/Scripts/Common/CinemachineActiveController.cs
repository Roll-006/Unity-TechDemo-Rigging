using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Rigging.Common
{
    public class CinemachineActiveController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachine;

        private void Awake()
        {
            if (cinemachine == null)
                throw new InvalidOperationException($"{nameof(cinemachine)} ({typeof(CinemachineCamera).Name}) の取得に失敗しました。SerializeField に登録してください。");
        }

        public void ActivateCinemachine()
        {
            cinemachine.enabled = true;
        }

        public void DeactivateCinemachine()
        {
            cinemachine.enabled = false;
        }
    }
}
