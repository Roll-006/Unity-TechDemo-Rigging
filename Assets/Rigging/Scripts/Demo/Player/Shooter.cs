using Riggin.Demo.Player;
using Rigging.Common;
using Rigging.Core;
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
        [SerializeField] private Transform gripPivot;
        [SerializeField] private Transform aimTrigger;
        [SerializeField] private Transform spine2Pivot;
        [SerializeField] private float towardSpeed;

        public bool IsAiming => _isAimingInput && weaponHolder.EquippedWeapon != null;

        private int _gunLayerIndex;
        private bool _isAimingInput;

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(playerInput, this);
            Debugger.I.ValidateSerializeField(aimGunAction, this);
            Debugger.I.ValidateSerializeField(fireAction, this);
            Debugger.I.ValidateSerializeField(animator, this);
            Debugger.I.ValidateSerializeField(aimRigUpdater, this);
            Debugger.I.ValidateSerializeField(weaponHolder, this);
            Debugger.I.ValidateSerializeField(gripPivot, this);
            Debugger.I.ValidateSerializeField(aimTrigger, this);
            Debugger.I.ValidateSerializeField(spine2Pivot, this);

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
                    break;
                    
                case InputActionPhase.Canceled:
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
    }
}
