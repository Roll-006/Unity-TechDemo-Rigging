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
            
            if (_aimArmBackMovement.BackArm(animator, _aimArmBackMovement.AimTrigger, _wasNotifiedTrigger)) _wasNotifiedTrigger = true;
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _wasNotifiedTrigger = false;
        }
    }
}
