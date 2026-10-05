using System;
using Rigging.Common;
using Rigging.Core;
using Rigging.Demo.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Demo.Player
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputActionReference aimGunAction;
        [SerializeField] private InputActionReference fireAction;
        [SerializeField] private Animator animator;
        [SerializeField] private AimRigUpdater aimRigUpdater;
        [SerializeField] private WeaponHolder weaponHolder;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private CameraPivotMovement cameraPivotMovement;
        [SerializeField] private Transform gripPivot;
        [SerializeField] private Transform aimTrigger;
        [SerializeField] private Transform spine2Pivot;
        [SerializeField] private float towardSpeed;

        public bool IsAiming => _isAimingInput && weaponHolder.EquippedWeapon != null;
        
        /// <summary>
        /// このフレームでエイム状態になった。
        /// </summary>
        public event Action AimInputThisFrameChanged;
        
        /// <summary>
        /// このフレームでエイム状態を解除した。
        /// </summary>
        public event Action ReleaseInputThisFrameChanged;

        private IReticleProvider _reticleProvider;
        private int _gunLayerIndex;
        private bool _isAimingInput;

        private void Awake()
        {
            Debugger.ValidateSerializeField(playerInput, this);
            Debugger.ValidateSerializeField(aimGunAction, this);
            Debugger.ValidateSerializeField(fireAction, this);
            Debugger.ValidateSerializeField(animator, this);
            Debugger.ValidateSerializeField(aimRigUpdater, this);
            Debugger.ValidateSerializeField(weaponHolder, this);
            Debugger.ValidateSerializeField(playerMovement, this);
            Debugger.ValidateSerializeField(cameraPivotMovement, this);
            Debugger.ValidateSerializeField(gripPivot, this);
            Debugger.ValidateSerializeField(aimTrigger, this);
            Debugger.ValidateSerializeField(spine2Pivot, this);

            _gunLayerIndex = animator.GetLayerIndex(AnimatorLayers.GunLayer);
        }
        
        private void OnEnable()
        {
            playerInput.onActionTriggered += OnActionTriggered;
        }

        private void OnDisable()
        {
            playerInput.onActionTriggered -= OnActionTriggered;
        }

        private void Update()
        {
            UpdateReticleSize();
        }

        private void LateUpdate()
        {
            UpdateAnimatorParameters();
            LayerWeightUpdater.UpdateLayerWeight(animator, _gunLayerIndex, IsActiveLayer(), towardSpeed);

            if (IsAiming)
            {
                aimRigUpdater.EnableAimRig();
            }
            else
            {
                aimRigUpdater.DisableAimRig();
            }
        }

        private void OnActionTriggered(InputAction.CallbackContext context)
        {
            if (OnAimGun(context)) return;
            if (OnFire(context)) return;
        }

        private bool OnAimGun(InputAction.CallbackContext context)
        {
            if (context.action != aimGunAction.action) return false;

            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _isAimingInput = true;
                    if (IsAiming) AimInputThisFrameChanged?.Invoke();
                    break;
                    
                case InputActionPhase.Canceled:
                    if (IsAiming) ReleaseInputThisFrameChanged?.Invoke();
                    _isAimingInput = false;
                    break;
            }

            return true;
        }

        private bool OnFire(InputAction.CallbackContext context)
        {
            if (context.action != fireAction.action) return false;

            return true;
        }

        private void UpdateAnimatorParameters()
        {
            animator.SetBool(AnimatorParameters.IsAiming, IsAiming);
            animator.SetBool(AnimatorParameters.CanAim, CanAim());
            animator.SetBool(AnimatorParameters.HasHandgun, weaponHolder.PendingWeapon != null && weaponHolder.PendingWeapon.WeaponType == WeaponType.Handgun);
            animator.SetBool(AnimatorParameters.HasRifle, weaponHolder.PendingWeapon != null && weaponHolder.PendingWeapon.WeaponType == WeaponType.Rifle);
        }

        private bool IsActiveLayer()
        {
            return weaponHolder.EquippedWeapon != null || weaponHolder.PendingWeapon != null;
        }

        private bool CanAim()
        {
            var spine2ToAimTrigger = aimTrigger.position - spine2Pivot.position;
            if (Physics.Raycast(
                    spine2Pivot.position,
                    spine2ToAimTrigger.normalized,
                    spine2ToAimTrigger.magnitude,
                    ~Layers.Player,
                    QueryTriggerInteraction.Ignore))
                return false;
            
            return true;
        }

        private void UpdateReticleSize()
        {
            if (!IsAiming) return;
            if (playerMovement.IsMoving)
            {
                
            }
        }
    }
}
