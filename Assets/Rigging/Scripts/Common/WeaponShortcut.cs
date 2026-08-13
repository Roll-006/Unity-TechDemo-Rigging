using Rigging.Core;
using Rigging.Player;
using System;
using UnityEngine;

public class WeaponShortcut : MonoBehaviour, IShortcut
{
    [SerializeField] private WeaponEquipper weaponEquipper;

    public WeaponBase Weapon { get; set; }
    public bool HasItem => Weapon != null;
    public bool CanSelect => HasItem;

    private void Awake()
    {
        if (weaponEquipper == null)
            throw new InvalidOperationException("WeaponEquipper ‚Ìæ“¾‚É¸”s‚µ‚Ü‚µ‚½BSerializeField ‚É“o˜^‚µ‚Ä‚­‚¾‚³‚¢B");
    }

    public void EquipWeapon()
    {
        weaponEquipper.OnEquipWeapon(Weapon);
    }
}
