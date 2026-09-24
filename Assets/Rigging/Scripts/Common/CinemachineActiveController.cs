using Rigging.Core;
using Unity.Cinemachine;
using UnityEngine;

namespace Rigging.Common
{
    public class CinemachineActiveController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachine;

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(cinemachine, this);
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
