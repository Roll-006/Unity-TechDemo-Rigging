using Rigging.Demo.Player;
using System;
using UnityEngine;

namespace Riggin.Demo.Player
{
    public class AimState : StateMachineBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerFacing _playerFacing;
        private AimRigUpdater _aimRigUpdater;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement = animator.GetComponent<PlayerMovement>() ??
                throw new InvalidOperationException($"{nameof(_playerMovement)} ({typeof(PlayerMovement).Name}) の取得に失敗しました。Component に登録してください。");

            _playerFacing = animator.GetComponent<PlayerFacing>() ??
                throw new InvalidOperationException($"{nameof(_playerFacing)} ({typeof(PlayerFacing).Name}) の取得に失敗しました。Component に登録してください。");

            _aimRigUpdater = animator.GetComponent<AimRigUpdater>() ??
                throw new InvalidOperationException($"{nameof(_aimRigUpdater)} ({typeof(AimRigUpdater).Name}) の取得に失敗しました。Component に登録してください。");

            _aimRigUpdater.EnableAimRig();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement.StopSprint();
            _playerFacing.SyncCameraForward();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _aimRigUpdater.DisableAimRig();
        }
    }
}
