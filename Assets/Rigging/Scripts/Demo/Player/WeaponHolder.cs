using Rigging.Core;
using System.Collections.Generic;
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

        public WeaponBase EquippedWeapon { get; private set; }
        public WeaponBase EquippedWeaponInstance { get; private set; }
        public WeaponBase PendingWeapon { get; private set; }

        private bool _isEquipped;
        private int _count;

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(animator, this);
            Debugger.I.ValidateSerializeField(rightHandGripPivot, this);
            Debugger.I.ValidateSerializeField(leftHandRigHint, this);
            Debugger.I.ValidateSerializeField(leftHandRigTarget, this);
        }

        private void Update()
        {
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

        public void OnEquipWeapon(WeaponBase weapon)
        {
            if (weapon == null) return;
            if (weapon == PendingWeapon) return;
            if (weapon == EquippedWeapon) return;

            PendingWeapon = weapon;

            animator.SetTrigger("EquipWeapon");
            if (PendingWeapon != EquippedWeapon && EquippedWeapon != null)
            {
                animator.SetTrigger("UnequipWeapon");
            }
        }

        public void OnUnequipWeapon()
        {
            if (PendingWeapon == null) return;
            if (EquippedWeapon == null) return;

            PendingWeapon = null;
            animator.SetTrigger("UnequipWeapon");
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
        }

        public void DestroyWeapon()
        {
            if (EquippedWeaponInstance == null) return;

            Destroy(EquippedWeaponInstance.gameObject);

            EquippedWeaponInstance = null;
            EquippedWeapon = null;
        }
    }
}
