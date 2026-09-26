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
        
        private bool _prevCrouching;

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(cinemachine, this);
            Debugger.I.ValidateSerializeField(playerMovement, this);
        }

        private void Update()
        {
            cinemachine.enabled = playerMovement.IsCrouching;
        }
    }
}
