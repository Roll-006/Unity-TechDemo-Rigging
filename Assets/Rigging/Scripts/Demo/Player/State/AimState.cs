using Rigging.Core;
using Rigging.Common;
using Rigging.Demo.Player;
using UnityEngine;

namespace Riggin.Demo.Player.State
{
    public class AimState : StateMachineBehaviour
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
            BackArm(animator);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _wasNotifiedTrigger = false;
        }
        
        private void BackArm(Animator animator)
        {
            if (_wasNotifiedTrigger) return;
            if (!_aimTriggerProvider.AimTrigger.IsHit) return;
            
            animator.SetTrigger(AnimatorParameters.BackArm);
            _wasNotifiedTrigger = true;
        }
    }
}
