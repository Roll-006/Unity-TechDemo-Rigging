using System;
using Rigging.Core;
using Rigging.Common;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Demo.Player
{
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Transform rightHandGripPivot;
        [SerializeField] private Transform leftHandRigHint;
        [SerializeField] private Transform leftHandRigTarget;
        [SerializeField] private List<WeaponBase> testWeapons;

        /// <summary>
        /// 装備している武器 (Hierarchy 上には実在していない)
        /// </summary>
        public WeaponBase EquippedWeapon { get; private set; }
        
        /// <summary>
        /// 装備することを発注した武器
        /// </summary>
        public WeaponBase PendingWeapon { get; private set; }

        /// <summary>
        /// Hierarchy 上に実在する装備している武器
        /// </summary>
        public WeaponBase EquippedWeaponInstance { get; private set; }
        
        /// <summary>
        /// Hierarchy 上に実在する装備している武器が変化したことを通知する。 (T: WeaponBase)
        /// </summary>
        public ReadOnlyReactiveProperty<WeaponBase> EquippedWeaponInstanceChanged => _equippedWeaponInstanceChanged;
        
        private readonly ReactiveProperty<WeaponBase> _equippedWeaponInstanceChanged = new ReactiveProperty<WeaponBase>();
        private bool _isEquipped; //仮
        private int _count; //仮

        private void Awake()
        {
            Debugger.ValidateSerializeField(animator, this);
            Debugger.ValidateSerializeField(rightHandGripPivot, this);
            Debugger.ValidateSerializeField(leftHandRigHint, this);
            Debugger.ValidateSerializeField(leftHandRigTarget, this);
        }

        private void Update()
        {
            // 仮の武器選択
            
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                _isEquipped = !_isEquipped;
            }

            if (_isEquipped)
            {
                OnEquipWeapon(testWeapons[_count]);
            }
            else
            {
                OnUnequipWeapon();
            }

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                _count = 0;
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                _count = 1;
            }
            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                _count = 2;
            }
            if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                _count = 3;
            }
        }

        private void OnDestroy()
        {
            _equippedWeaponInstanceChanged.Dispose();
        }

        public void OnEquipWeapon(WeaponBase weapon)
        {
            if (weapon == null) return;
            if (weapon == PendingWeapon) return;
            if (weapon == EquippedWeapon) return;

            PendingWeapon = weapon;

            animator.SetTrigger(AnimatorParameters.EquipWeapon);
            if (PendingWeapon != EquippedWeapon && EquippedWeapon != null)
            {
                animator.SetTrigger(AnimatorParameters.UnequipWeapon);
            }
        }

        public void OnUnequipWeapon()
        {
            if (PendingWeapon == null) return;
            if (EquippedWeapon == null) return;

            PendingWeapon = null;
            animator.SetTrigger(AnimatorParameters.UnequipWeapon);
        }

        public void CreateWeapon()
        {
            if (PendingWeapon == null) return;

            EquippedWeapon = PendingWeapon;

            rightHandGripPivot.localPosition = EquippedWeapon.Rig.PivotOffset;
            leftHandRigHint.localPosition = EquippedWeapon.Rig.LeftHandRigHintPosition;
            leftHandRigTarget.localPosition = EquippedWeapon.Rig.LeftHandRigTargetPosition;
            leftHandRigTarget.localEulerAngles = EquippedWeapon.Rig.LeftHandRigTargetRotation;

            EquippedWeaponInstance = Instantiate(EquippedWeapon, rightHandGripPivot, false);

            var shootable = EquippedWeaponInstance.GetComponent<IShootable>();
            if (shootable != null)
            {
                shootable.AimTarget = aimTarget;
            }
            
            _equippedWeaponInstanceChanged.Value = EquippedWeaponInstance;
        }

        public void DestroyWeapon()
        {
            if (EquippedWeaponInstance == null) return;

            Destroy(EquippedWeaponInstance.gameObject);

            EquippedWeaponInstance = null;
            EquippedWeapon = null;
            
            _equippedWeaponInstanceChanged.Value = EquippedWeaponInstance;
        }
    }
}