using Rigging.Core;
using UnityEngine;
using Unity.Cinemachine;

namespace Rigging.Demo.Characters.Player
{
    public class CrouchCinemachinePriorityUpdater : MonoBehaviour
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
            cinemachine.Priority = playerMovement.IsCrouching ? 1 : 0;
        }
    }
}