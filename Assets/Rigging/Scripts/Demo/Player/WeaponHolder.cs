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
        public WeaponBase EquippedWeaponInstance { get; private set; }
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

            Debug.Log("装備が要求が承認されました。");
            Debug.Log($"PendingWeapon : {PendingWeapon}");
            Debug.Log($"EquippedWeapon : {EquippedWeapon}");
        }

        public void OnUnequipWeapon()
        {
            if (PendingWeapon == null) return;
            if (EquippedWeapon == null) return;

            PendingWeapon = null;
            animator.SetTrigger("UnequipWeapon");

            Debug.Log("着脱が要求が承認されました。");
            Debug.Log($"PendingWeapon : {PendingWeapon}");
            Debug.Log($"EquippedWeapon : {EquippedWeapon}");
        }

        public void CreateWeapon()
        {
            Debug.Log("装備通知が送られました。");

            if (PendingWeapon == null) return;

            EquippedWeapon = PendingWeapon;
            handGripPivot.localPosition = EquippedWeapon.PivotOffset;
            EquippedWeaponInstance = Instantiate(EquippedWeapon, handGripPivot, false);

            Debug.Log("装備がされました。");
            Debug.Log($"PendingWeapon : {PendingWeapon}");
            Debug.Log($"EquippedWeapon : {EquippedWeapon}");
        }

        public void DestroyWeapon()
        {
            Debug.Log("着脱通知が送られました。");

            if (EquippedWeaponInstance == null) return;

            Destroy(EquippedWeaponInstance.gameObject);
            EquippedWeaponInstance = null;
            EquippedWeapon = null;

            Debug.Log("着脱されました。");
            Debug.Log($"PendingWeapon : {PendingWeapon}");
            Debug.Log($"EquippedWeapon : {EquippedWeapon}");
        }
    }
}
