using Rigging.Core;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Player
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputActionReference aimGunAction;
        [SerializeField] private InputActionReference fireAction;
        [SerializeField] private Animator animator;
        [SerializeField] private WeaponEquipper weaponEquipper;
        [SerializeField] private Transform gripPivot;
        [SerializeField] private float towardSpeed;

        public bool IsAiming {  get; private set; }

        private int _gunLayerIndex;

        private void Awake()
        {
            if (playerInput == null)
                throw new InvalidOperationException("PlayerInput ÇÃéÊìæÇ…é∏îsÇµÇ‹ÇµÇΩÅBSerializeField Ç…ìoò^ÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB");
            
            if (aimGunAction == null)
                throw new InvalidOperationException("Aim Gun Action ÇÃéÊìæÇ…é∏îsÇµÇ‹ÇµÇΩÅBSerializeField Ç…ìoò^ÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB");

            if (fireAction == null)
                throw new InvalidOperationException("Fire Action ÇÃéÊìæÇ…é∏îsÇµÇ‹ÇµÇΩÅBSerializeField Ç…ìoò^ÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB");

            if (animator == null)
                throw new InvalidOperationException("Animator ÇÃéÊìæÇ…é∏îsÇµÇ‹ÇµÇΩÅBSerializeField Ç…ìoò^ÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB");

            if (weaponEquipper == null)
                throw new InvalidOperationException("WeaponEquipper ÇÃéÊìæÇ…é∏îsÇµÇ‹ÇµÇΩÅBSerializeField Ç…ìoò^ÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB");

            if (gripPivot == null)
                throw new InvalidOperationException("Grip Pivot ÇÃéÊìæÇ…é∏îsÇµÇ‹ÇµÇΩÅBSerializeField Ç…ìoò^ÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB");

            _gunLayerIndex = animator.GetLayerIndex("Gun Layer");
            if (_gunLayerIndex == -1)
                throw new InvalidOperationException("Gun Layer ÇÃ Index ÇÃéÊìæÇ…é∏îsÇµÇ‹ÇµÇΩÅB");
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
            UpdateAnimatorParameters();
            UpdateLayerWeight();
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
                    IsAiming = true;
                    break;

                case InputActionPhase.Canceled:
                    IsAiming = false;
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
            animator.SetBool("IsAiming", IsAiming);
            animator.SetBool("HasHandgun", weaponEquipper.EquipWeapon ? weaponEquipper.EquipWeapon.WeaponType == WeaponType.Handgun : false);
            animator.SetBool("HasRifle", weaponEquipper.EquipWeapon ? weaponEquipper.EquipWeapon.WeaponType == WeaponType.Rifle : false);
        }

        private void UpdateLayerWeight()
        {
            var currentLayerWeight = animator.GetLayerWeight(_gunLayerIndex);
            var targetLayerWeight = weaponEquipper.EquipWeapon ? 1 : 0;
            var resultLayerWeight = Mathf.MoveTowards(currentLayerWeight, targetLayerWeight, towardSpeed * Time.deltaTime);

            animator.SetLayerWeight(_gunLayerIndex, resultLayerWeight);
        }
    }
}
