using System.Collections.Generic;
using R3;
using Rigging.Core;
using Unity.Cinemachine;
using UnityEngine;

namespace Rigging.Demo.Characters.Player
{
    public class PlayerCinemachinePriorityOperator : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera standCinemachine;
        [SerializeField] private CinemachineCamera sprintCinemachine;
        [SerializeField] private CinemachineCamera crouchCinemachine;
        [SerializeField] private CinemachineCamera aimCinemachine;
        [SerializeField] private Shooter shooter;
        [SerializeField] private PlayerMovement playerMovement;

        private void Awake()
        {
            Debugger.ValidateSerializeField(standCinemachine, this);
            Debugger.ValidateSerializeField(sprintCinemachine, this);
            Debugger.ValidateSerializeField(crouchCinemachine, this);
            Debugger.ValidateSerializeField(aimCinemachine, this);
            Debugger.ValidateSerializeField(shooter, this);
            Debugger.ValidateSerializeField(playerMovement, this);
        }

        private void Start()
        {
            shooter.IsAiming
                .Subscribe(OnOperatorPriorityAimCinemachine)
                .AddTo(this);
        }

        private void Update()
        {
            if (shooter.IsAiming.CurrentValue) return;
            
            if (playerMovement.IsMoving && playerMovement.IsSprinting)
            {
                CameraManager.I.Push(sprintCinemachine);
            }
            else if (playerMovement.IsCrouching)
            {
                CameraManager.I.Push(crouchCinemachine);
            }
            else
            {
                CameraManager.I.Push(standCinemachine);
            }
        }

        private void OnOperatorPriorityAimCinemachine(bool isAiming)
        {
            if (isAiming)
            {
                CameraManager.I.Push(aimCinemachine);
            }
            else
            {
                CameraManager.I.Remove(aimCinemachine);
            }
        }
    }
}