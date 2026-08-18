using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Player
{
    public class WeaponEquipper : MonoBehaviour
    {
        public WeaponBase EquippedWeapon { get; private set; }

        public void OnEquipWeapon(WeaponBase weapon)
        {
            if (weapon == null) return;

            EquippedWeapon = weapon;
        }

        public void OnUnequipWeapon()
        {
            EquippedWeapon = null;
        }
    }
}
