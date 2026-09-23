using Rigging.Core;
using Rigging.Common;
using UnityEngine;

namespace Rigging.Demo.Player.State
{
    public class ArmBackState : StateMachineBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerFacing _playerFacing;
        private AimTriggerProvider _aimTriggerProvider;
        private bool _wasNotifiedTrigger;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement = Debugger.I.ValidateComponent<PlayerMovement>(animator);
            _playerFacing = Debugger.I.ValidateComponent<PlayerFacing>(animator);
            _aimTriggerProvider = Debugger.I.ValidateComponent<AimTriggerProvider>(animator);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement.StopSprint();
            _playerFacing.SyncCameraForward();
            ExtendArm(animator);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _wasNotifiedTrigger = false;
        }

        private void ExtendArm(Animator animator)
        {
            if (_wasNotifiedTrigger) return;
            
            if (Physics.CheckCapsule(
                    _aimTriggerProvider.AimArmBackTrigger.transform.position,
                    _aimTriggerProvider.AimTrigger.transform.position,
                    0.1f,
                    ~LayerMask.GetMask("Player"),
                    QueryTriggerInteraction.Ignore))
                return;

            animator.SetTrigger(AnimatorParameters.ExtendArm);
            _wasNotifiedTrigger = true;
        }
    }
}
