using Rigging.Core;
using Rigging.Demo.Player;
using UnityEngine;
using Unity.Cinemachine;

namespace Rigging.Demo
{
    public class CrouchCinemachineEnabler : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachine;
        [SerializeField] private PlayerMovement playerMovement;

        private void Awake()
        {
            Debugger.ValidateSerializeField(cinemachine, this);
            Debugger.ValidateSerializeField(playerMovement, this);
        }

        private void Update()
        {
            cinemachine.enabled = playerMovement.IsCrouching;
        }
    }
}
