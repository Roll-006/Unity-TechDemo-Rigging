using Rigging.Core;
using Rigging.Common;
using Rigging.Demo.Player;
using System;
using UnityEngine;

namespace Riggin.Demo.Player
{
    public class LowerBodyTurn : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private CameraPivotMovement cameraPivotMovement;
        [SerializeField] private PlayerFacing playerFacing;
        [SerializeField] private float turnSpeedMultiplier;
        [SerializeField] private float towardSpeed;
        
        private int _lowerBodyTurnLayerIndex;

        void Awake()
        {
            if (animator == null)
                throw new InvalidOperationException($"{nameof(animator)} ({typeof(Animator).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (playerMovement == null)
                throw new InvalidOperationException($"{nameof(playerMovement)} ({typeof(PlayerMovement).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (cameraPivotMovement == null)
                throw new InvalidOperationException($"{nameof(cameraPivotMovement)} ({typeof(CameraPivotMovement).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (playerFacing == null)
                throw new InvalidOperationException($"{nameof(playerFacing)} ({typeof(PlayerFacing).Name}) の取得に失敗しました。SerializeField に登録してください。");

            _lowerBodyTurnLayerIndex = animator.GetLayerIndex("Lower Body Turn Layer");
            if (_lowerBodyTurnLayerIndex == -1)
                throw new InvalidOperationException($"{nameof(_lowerBodyTurnLayerIndex)} の Index の取得に失敗しました。");
        }

        void LateUpdate()
        {
            UpdateAnimatorParameters();
            UpdateLayerWeight();
        }

        private void UpdateAnimatorParameters()
        {
            animator.SetFloat("CameraMoveDirectionX", cameraPivotMovement.Input.x);
            animator.SetFloat("TurnSpeed", Mathf.Abs(cameraPivotMovement.Input.x) * turnSpeedMultiplier);
        }

        private void UpdateLayerWeight()
        {
            var currentLayerWeight = animator.GetLayerWeight(_lowerBodyTurnLayerIndex);
            var targetLayerWeight = IsActiveLayer() ? 1f : 0f;
            var resultLayerWeight = Mathf.MoveTowards(currentLayerWeight, targetLayerWeight, towardSpeed * Time.deltaTime);

            animator.SetLayerWeight(_lowerBodyTurnLayerIndex, resultLayerWeight);
        }

        private bool IsActiveLayer()
        {
            if (playerMovement.IsMoving) return false;
            if (!cameraPivotMovement.IsMoving) return false;
            if (!playerFacing.IsSyncCameraForward) return false;

            return true;
        }
    }
}
