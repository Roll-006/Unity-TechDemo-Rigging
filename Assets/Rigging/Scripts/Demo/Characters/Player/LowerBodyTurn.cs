using System.Collections.Generic;
using Rigging.Core;
using Rigging.Common;
using UnityEngine;

namespace Rigging.Demo.Characters.Player
{
    public class LowerBodyTurn : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private List<CameraPivotMovement> cameraPivotMovements;
        [SerializeField] private PlayerFacing playerFacing;
        [SerializeField] private float turnSpeedMultiplier;
        [SerializeField] private float towardSpeed;

        private float TurnSpeed
        {
            get
            {
                foreach (var cameraPivotMovement in cameraPivotMovements)
                {
                    if (!cameraPivotMovement.enabled) continue;
                    return Mathf.Abs(cameraPivotMovement.Input.x) * turnSpeedMultiplier;
                }
                return 0f;
            }
        }
        
        private int _lowerBodyTurnLayerIndex;

        void Awake()
        {
            Debugger.ValidateSerializeField(animator, this);
            Debugger.ValidateSerializeField(playerMovement, this);
            Debugger.ValidateSerializeField(playerFacing, this);

            _lowerBodyTurnLayerIndex = animator.GetLayerIndex(AnimatorLayers.LowerBodyTurnLayer);
        }

        private void Update()
        {
            UpdateAnimatorParameters();
            LayerWeightUpdater.UpdateLayerWeight(animator, _lowerBodyTurnLayerIndex, IsActiveLayer(), towardSpeed);
        }

        private void UpdateAnimatorParameters()
        {
            foreach (var cameraPivotMovement in cameraPivotMovements)
            {
                if (!cameraPivotMovement.enabled) continue;
                animator.SetFloat(AnimatorParameters.CameraMoveDirectionX, cameraPivotMovement.Input.x);
            }
            
            animator.SetFloat(AnimatorParameters.TurnSpeed, TurnSpeed);
        }
        
        private bool IsActiveLayer()
        {
            if (playerMovement.IsMoving) return false;
            if (!playerFacing.IsSyncCameraForward) return false;

            foreach (var cameraPivotMovement in cameraPivotMovements)
            {
                if (!cameraPivotMovement.enabled) continue;
                if (!cameraPivotMovement.IsMoving) return false;
            }

            return true;
        }
    }
}