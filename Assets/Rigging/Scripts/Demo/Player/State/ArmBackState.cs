using Rigging.Core;
using Rigging.Common;
using UnityEngine;

namespace Rigging.Demo.Player.State
{
    public class ArmBackState : StateMachineBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerFacing _playerFacing;
        private AimArmBackMovement _aimArmBackMovement;
        private bool _wasNotifiedTrigger;
        
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement = Debugger.I.ValidateComponent<PlayerMovement>(animator);
            _playerFacing = Debugger.I.ValidateComponent<PlayerFacing>(animator);
            _aimArmBackMovement = Debugger.I.ValidateComponent<AimArmBackMovement>(animator);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement.StopSprint();
            _playerFacing.SyncCameraForward();
            
            if (_aimArmBackMovement.ExtendArm(animator, _aimArmBackMovement.AimBackTrigger, _aimArmBackMovement.AimArmBackTrigger, _wasNotifiedTrigger)) _wasNotifiedTrigger = true;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _wasNotifiedTrigger = false;
        }
    }
}
