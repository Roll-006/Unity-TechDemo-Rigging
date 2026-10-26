using Rigging.Core;
using Unity.Cinemachine;
using UnityEngine;

namespace Rigging.Demo.Characters.Player
{
    public class SprintCinemachinePriorityUpdater : MonoBehaviour
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
            cinemachine.Priority = playerMovement.IsSprinting && playerMovement.IsMoving ? 1 : 0;
        }
    }
}