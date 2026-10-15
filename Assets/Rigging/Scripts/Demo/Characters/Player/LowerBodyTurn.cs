using Rigging.Core;
using Rigging.Common;
using UnityEngine;

namespace Rigging.Demo.Characters.Player
{
    public class LowerBodyTurn : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private CameraPivotMovement cameraPivotMovement;
        [SerializeField] private PlayerFacing playerFacing;
        [SerializeField] private float turnSpeedMultiplier;
        [SerializeField] private float towardSpeed;
        
        private float TurnSpeed => Mathf.Abs(cameraPivotMovement.Input.x) * turnSpeedMultiplier;
        
        private int _lowerBodyTurnLayerIndex;

        void Awake()
        {
            Debugger.ValidateSerializeField(animator, this);
            Debugger.ValidateSerializeField(playerMovement, this);
            Debugger.ValidateSerializeField(cameraPivotMovement, this);
            Debugger.ValidateSerializeField(playerFacing, this);

            _lowerBodyTurnLayerIndex = animator.GetLayerIndex(AnimatorLayers.LowerBodyTurnLayer);
        }

        void LateUpdate()
        {
            UpdateAnimatorParameters();
            LayerWeightUpdater.UpdateLayerWeight(animator, _lowerBodyTurnLayerIndex, IsActiveLayer(), towardSpeed);
        }

        private void UpdateAnimatorParameters()
        {
            animator.SetFloat(AnimatorParameters.CameraMoveDirectionX, cameraPivotMovement.Input.x);
            animator.SetFloat(AnimatorParameters.TurnSpeed, TurnSpeed);
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