using Rigging.Common;
using Rigging.Core;
using Rigging.Demo.Player;
using System;
using UnityEngine;

namespace Riggin.Demo.Player
{
    public class AimState : StateMachineBehaviour
    {
        private CinemachineActiveController _cinemachineActiveController;
        private PlayerMovement _playerMovement;
        private PlayerFacing _playerFacing;
        private AimRigUpdater _aimRigUpdater;
        private RightHandRigUpdater _rightHandRigUpdater;
        private ITriggerable _weapon;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _cinemachineActiveController = animator.GetComponent<CinemachineActiveController>() ??
                throw new InvalidOperationException($"{nameof(_cinemachineActiveController)} ({typeof(CinemachineActiveController).Name}) の取得に失敗しました。Component に登録してください。");

            _playerMovement = animator.GetComponent<PlayerMovement>() ??
                throw new InvalidOperationException($"{nameof(_playerMovement)} ({typeof(PlayerMovement).Name}) の取得に失敗しました。Component に登録してください。");

            _playerFacing = animator.GetComponent<PlayerFacing>() ??
                throw new InvalidOperationException($"{nameof(_playerFacing)} ({typeof(PlayerFacing).Name}) の取得に失敗しました。Component に登録してください。");

            _aimRigUpdater = animator.GetComponent<AimRigUpdater>() ??
                throw new InvalidOperationException($"{nameof(_aimRigUpdater)} ({typeof(AimRigUpdater).Name}) の取得に失敗しました。Component に登録してください。");

            _rightHandRigUpdater = animator.GetComponent<RightHandRigUpdater>() ??
                throw new InvalidOperationException($"{nameof(_rightHandRigUpdater)} ({typeof(RightHandRigUpdater).Name}) の取得に失敗しました。Component に登録してください。");

            var weappnHolder = animator.GetComponent<WeaponHolder>() ??
                throw new InvalidOperationException($"({typeof(WeaponHolder).Name}) の取得に失敗しました。Component に登録してください。");

            _weapon = weappnHolder.EquippedWeaponInstance.GetComponent<ITriggerable>();

            _cinemachineActiveController.ActivateCinemachine();
            _aimRigUpdater.EnableAimRig();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement.StopSprint();
            _playerFacing.SyncCameraForward();
            PushBack();

        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _cinemachineActiveController.DeactivateCinemachine();
            _aimRigUpdater.DisableAimRig();
            _rightHandRigUpdater.DisableRightHandRig();
        }

        private void PushBack()
        {
            if (_weapon == null) return;

            if (_weapon.RayPivotTrigger.IsHit)
            {
                Debug.Log("衝突しました");
                _rightHandRigUpdater.EnableRightHandRig();
            }
            else
            {
                Debug.Log("解除されました");
                _rightHandRigUpdater.DisableRightHandRig();
            }
        }
    }
}
