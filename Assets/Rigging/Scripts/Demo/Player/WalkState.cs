using System;
using UnityEngine;

namespace Rigging.Demo.Player
{
    public class WalkState : StateMachineBehaviour
    {
        private PlayerFacing _playerFacing;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerFacing = animator.GetComponent<PlayerFacing>() ??
                throw new InvalidOperationException($"{nameof(_playerFacing)} ({typeof(PlayerFacing).Name}) の取得に失敗しました。Component に登録してください。"); ;

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
