using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Player.State
{
    public class AimState : StateMachineBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerFacing _playerFacing;
        private AimArmBackMovement _aimArmBackMovement;
        private bool _wasNotifiedTrigger;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement = Debugger.I.ValidateComponent<PlayerMovement>(animator);
            _playerFacing = Debugger.I.ValidateComponent<PlayerFacing>(animator);
            _aimArmBackMovement = Debugger.I.ValidateComponent<AimArmBackMovement>(animator);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement.StopSprint();
            _playerFacing.SyncCameraForward();
            
            if (_aimArmBackMovement.BackArm(animator, _aimArmBackMovement.AimTrigger, _wasNotifiedTrigger)) _wasNotifiedTrigger = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _wasNotifiedTrigger = false;
        }
    }
}
