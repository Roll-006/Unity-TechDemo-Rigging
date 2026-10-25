using Rigging.Common;
using Rigging.Core;
using UnityEngine;
using Unity.Cinemachine;

namespace Rigging.Demo.Characters.Player
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