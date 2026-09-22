using Riggin.Demo.Player;
using Rigging.Common;
using Rigging.Core;
using System;
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
        [SerializeField] private CinemachineActiveController cinemachineActiveController;
        [SerializeField] private AimRigUpdater aimRigUpdater;
        [SerializeField] private WeaponHolder weaponHolder;
        [SerializeField] private Transform gripPivot;
        [SerializeField] private float towardSpeed;

        public bool IsAiming => _isAimingInput && weaponHolder.EquippedWeapon != null;

        private int _gunLayerIndex;
        private bool _isAimingInput;
        private int _isAimingIndex;
        private int _isHitWeaponRayPivotIndex;
        private int _hasHandgunIndex;
        private int _hasRifleIndex;

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(playerInput, this);
            Debugger.I.ValidateSerializeField(aimGunAction, this);
            Debugger.I.ValidateSerializeField(fireAction, this);
            Debugger.I.ValidateSerializeField(animator, this);
            Debugger.I.ValidateSerializeField(cinemachineActiveController, this);
            Debugger.I.ValidateSerializeField(aimRigUpdater, this);
            Debugger.I.ValidateSerializeField(weaponHolder, this);
            Debugger.I.ValidateSerializeField(gripPivot, this);

            _gunLayerIndex = animator.GetLayerIndex("Gun Layer");
            if (_gunLayerIndex == -1)
                throw new InvalidOperationException($"{nameof(_gunLayerIndex)} の Index の取得に失敗しました。");

            _isAimingIndex = Animator.StringToHash("IsAiming");
            _isHitWeaponRayPivotIndex = Animator.StringToHash("IsHitWeaponRayPivot");
            _hasHandgunIndex = Animator.StringToHash("HasHandgun");
            _hasRifleIndex = Animator.StringToHash("HasRifle");
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
            UpdateLayerWeight();

            if (IsAiming)
            {
                cinemachineActiveController.ActivateCinemachine();
                aimRigUpdater.EnableAimRig();
            }
            else
            {
                cinemachineActiveController.DeactivateCinemachine();
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

            animator.SetBool(_isAimingIndex, IsAiming);
            if (weaponHolder.EquippedWeaponInstance) animator.SetBool(_isHitWeaponRayPivotIndex, weaponHolder.EquippedWeaponInstance.GetComponent<ITriggerable>().RayPivotTrigger.IsHit);
            animator.SetBool(_hasHandgunIndex, weaponHolder.PendingWeapon ? weaponHolder.PendingWeapon.WeaponType == WeaponType.Handgun : false);
            animator.SetBool(_hasRifleIndex, weaponHolder.PendingWeapon ? weaponHolder.PendingWeapon.WeaponType == WeaponType.Rifle : false);
        }

        private void UpdateLayerWeight()
        {
            var currentLayerWeight = animator.GetLayerWeight(_gunLayerIndex);
            var targetLayerWeight = IsActiveGunLayer() ? 1f : 0f;
            var resultLayerWeight = Mathf.MoveTowards(currentLayerWeight, targetLayerWeight, towardSpeed * Time.deltaTime);

            animator.SetLayerWeight(_gunLayerIndex, resultLayerWeight);
        }

        private bool IsActiveGunLayer()
        {
            return weaponHolder.EquippedWeapon != null || weaponHolder.PendingWeapon != null;
        }
    }
}
