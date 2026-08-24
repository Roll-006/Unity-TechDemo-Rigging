using Rigging.Common;
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
        private CinemachineActiveController _cinemachineActiveController;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement = animator.GetComponent<PlayerMovement>() ??
                throw new InvalidOperationException($"{nameof(_playerMovement)} ({typeof(PlayerMovement).Name}) の取得に失敗しました。Component に登録してください。");

            _playerFacing = animator.GetComponent<PlayerFacing>() ??
                throw new InvalidOperationException($"{nameof(_playerFacing)} ({typeof(PlayerFacing).Name}) の取得に失敗しました。Component に登録してください。");

            _aimRigUpdater = animator.GetComponent<AimRigUpdater>() ??
                throw new InvalidOperationException($"{nameof(_aimRigUpdater)} ({typeof(AimRigUpdater).Name}) の取得に失敗しました。Component に登録してください。");

            _cinemachineActiveController = animator.GetComponent<CinemachineActiveController>() ??
                throw new InvalidOperationException($"{nameof(_cinemachineActiveController)} ({typeof(CinemachineActiveController).Name}) の取得に失敗しました。Component に登録してください。");

            _aimRigUpdater.EnableAimRig();
            _cinemachineActiveController.ActivateCinemachine();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement.StopSprint();
            _playerFacing.SyncCameraForward();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _aimRigUpdater.DisableAimRig();
            _cinemachineActiveController.DeactivateCinemachine();
        }
    }
}
