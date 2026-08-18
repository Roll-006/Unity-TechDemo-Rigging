using Rigging.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Demo.Player
{
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform handGripPivot;
        [SerializeField] private List<WeaponBase> testWeapons;

        public WeaponBase EquippedWeapon { get; private set; }
        public WeaponBase PendingWeapon { get; private set; }

        private bool _isEquipped;
        private int _count;

        private void Awake()
        {
            if (animator == null)
                throw new InvalidOperationException($"{nameof(animator)} ({typeof(Animator).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (handGripPivot == null)
                throw new InvalidOperationException($"{nameof(handGripPivot)} ({typeof(Transform).Name}) の取得に失敗しました。SerializeField に登録してください。");
        }

        private void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                _isEquipped = !_isEquipped;
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

            if (_isEquipped)
            {
                OnEquipWeapon(testWeapons[_count]);
            }
            else
            {
                OnUnequipWeapon();
            }
        }

        public void OnEquipWeapon(WeaponBase weapon)
        {
            if (weapon == null) return;
            if (weapon == PendingWeapon) return;
            if (weapon == EquippedWeapon) return;
            Debug.Log("装備が要求されました。");
            Debug.Log($"PendingWeapon : {PendingWeapon}");
            Debug.Log($"EquippedWeapon : {EquippedWeapon}");

            PendingWeapon = weapon;

            animator.SetTrigger("EquipWeapon");
        }

        public void OnUnequipWeapon()
        {
            if (PendingWeapon == null) return;
            if (EquippedWeapon == null) return;
            Debug.Log("着脱が要求されました。");
            Debug.Log($"PendingWeapon : {PendingWeapon}");
            Debug.Log($"EquippedWeapon : {EquippedWeapon}");

            PendingWeapon = null;

            animator.SetTrigger("UnequipWeapon");
        }

        public void CreateWeapon()
        {
            Debug.Log("装備がされました。");
            Debug.Log($"PendingWeapon : {PendingWeapon}");
            Debug.Log($"EquippedWeapon : {EquippedWeapon}");

            EquippedWeapon = PendingWeapon;
            handGripPivot.localPosition = EquippedWeapon.PivotOffset;

            EquippedWeapon = Instantiate(PendingWeapon);
            EquippedWeapon.transform.SetParent(handGripPivot, false);
        }

        public void DestroyWeapon()
        {
            if (EquippedWeapon == null) return;
            Debug.Log("着脱されました。");
            Debug.Log($"PendingWeapon : {PendingWeapon}");
            Debug.Log($"EquippedWeapon : {EquippedWeapon}");

            Destroy(EquippedWeapon.gameObject);
            EquippedWeapon = null;
        }
    }
}
