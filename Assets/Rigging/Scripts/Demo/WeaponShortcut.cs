using Rigging.Core;
using Rigging.Demo.Player;
using System;
using UnityEngine;

namespace Rigging.Demo
{
    public class WeaponShortcut : MonoBehaviour, IShortcut
    {
        [SerializeField] private WeaponHolder weaponHolder;

        public WeaponBase Weapon { get; set; }
        public bool HasItem => Weapon != null;
        public bool CanSelect => HasItem;

        private void Awake()
        {
            if (weaponHolder == null)
                throw new InvalidOperationException($"{nameof(weaponHolder)} ({typeof(WeaponHolder).Name}) の取得に失敗しました。SerializeField に登録してください。");
        }

        public void EquipWeapon()
        {
            weaponHolder.OnEquipWeapon(Weapon);
        }
    }
}
