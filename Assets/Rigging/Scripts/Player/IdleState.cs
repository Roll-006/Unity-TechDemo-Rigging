using UnityEngine;

namespace Rigging.Player
{
    public class IdleState : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<PlayerMovement>()?.StopSprint();
        }
    }
}
