using Rigging.Demo.Player;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Riggin.Demo.Player
{
    public class AimState : StateMachineBehaviour
    {
        private PlayerFacing _playerFacing;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerFacing = animator.GetComponent<PlayerFacing>() ??
                throw new InvalidOperationException($"{nameof(_playerFacing)} ({typeof(PlayerFacing).Name}) の取得に失敗しました。Component に登録してください。"); ;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerFacing.SyncCameraForward();
        }
    }
}
