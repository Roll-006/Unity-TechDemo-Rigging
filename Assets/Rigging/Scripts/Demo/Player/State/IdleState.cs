using UnityEngine;

namespace Rigging.Demo.Player.State
{
    public class IdleState : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<PlayerMovement>()?.StopSprint();
        }
    }
}
