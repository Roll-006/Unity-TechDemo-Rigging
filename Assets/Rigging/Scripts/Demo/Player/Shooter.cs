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
        [SerializeField] private WeaponHolder weaponHolder;
        [SerializeField] private Transform gripPivot;
        [SerializeField] private float towardSpeed;

        public bool IsAiming => _isAimingInput && weaponHolder.EquippedWeapon != null;

        private int _gunLayerIndex;
        private bool _isAimingInput;

        private void Awake()
        {
            if (playerInput == null)
                throw new InvalidOperationException($"{nameof(playerInput)} ({typeof(PlayerInput).Name}) の取得に失敗しました。SerializeField に登録してください。");
            
            if (aimGunAction == null)
                throw new InvalidOperationException($"{nameof(aimGunAction)} ({typeof(InputActionReference).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (fireAction == null)
                throw new InvalidOperationException($"{nameof(fireAction)} ({typeof(InputActionReference).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (animator == null)
                throw new InvalidOperationException($"{nameof(animator)} ({typeof(Animator).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (weaponHolder == null)
                throw new InvalidOperationException($"{nameof(weaponHolder)} ({typeof(WeaponHolder).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (gripPivot == null)
                throw new InvalidOperationException($"{nameof(gripPivot)} ({typeof(Transform).Name}) の取得に失敗しました。SerializeField に登録してください。");

            _gunLayerIndex = animator.GetLayerIndex("Gun Layer");
            if (_gunLayerIndex == -1)
                throw new InvalidOperationException($"{nameof(_gunLayerIndex)} の Index の取得に失敗しました。");
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
            animator.SetBool("IsAiming", IsAiming);
            animator.SetBool("HasHandgun", weaponHolder.PendingWeapon ? weaponHolder.PendingWeapon.WeaponType == WeaponType.Handgun : false);
            animator.SetBool("HasRifle", weaponHolder.PendingWeapon ? weaponHolder.PendingWeapon.WeaponType == WeaponType.Rifle : false);
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
