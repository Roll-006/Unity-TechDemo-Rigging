using Rigging.Core;
using Rigging.Demo.Characters.Player;
using Rigging.Demo.Weapons;
using UnityEngine;

namespace Rigging.Demo.Shortcut
{
    public class WeaponShortcut : MonoBehaviour, IShortcut
    {
        [SerializeField] private WeaponHolder weaponHolder;

        public WeaponBase Weapon { get; set; }
        public bool HasItem => Weapon != null;
        public bool CanSelect => HasItem;

        private void Awake()
        {
            Debugger.ValidateSerializeField(weaponHolder, this);
        }

        public void EquipWeapon()
        {
            weaponHolder.OnEquipWeapon(Weapon);
        }
    }
}