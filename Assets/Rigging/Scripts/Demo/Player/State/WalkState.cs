using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Player.State
{
    public class WalkState : StateMachineBehaviour
    {
        private PlayerFacing _playerFacing;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerFacing = Debugger.ValidateComponent<PlayerFacing>(animator);

            animator.GetComponent<PlayerMovement>()?.StopSprint();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerFacing.SyncCameraForward();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}