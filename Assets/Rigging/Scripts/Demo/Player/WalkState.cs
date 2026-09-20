using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Player
{
    public class WalkState : StateMachineBehaviour
    {
        private PlayerFacing _playerFacing;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerFacing = Debugger.I.ValidateComponent<PlayerFacing>(animator);

            animator.GetComponent<PlayerMovement>()?.StopSprint();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerFacing.SyncCameraForward();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}
