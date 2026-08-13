using Rigging.Core;
using UnityEngine;

namespace Rigging.Player
{
    public class WeaponEquipper : MonoBehaviour
    {
        public WeaponBase EquipWeapon { get; private set; }

        public void OnEquipWeapon(WeaponBase weapon)
        {
            if (weapon == null) return;

            EquipWeapon = weapon;
        }

        public void OnUnequip()
        {
            EquipWeapon = null;
        }
    }
}
