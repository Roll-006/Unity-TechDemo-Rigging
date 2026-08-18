using Rigging.Core;
using System;
using UnityEngine;

namespace Rigging.Demo.Weapons
{
    public class Rifle : WeaponBase, IInventoryStorable, IReloadable
    {
        [SerializeField] private Vector3 pivotOffset;
        [SerializeField, Min(1)] private Vector2Int gridSize;
        [SerializeField] private Sprite icon;

        public override WeaponType WeaponType => WeaponType.Rifle;
        public override Vector3 PivotOffset => pivotOffset;
        public Vector2Int GridSize => gridSize;
        public Sprite Icon => icon;

        private void Awake()
        {
            if (icon == null)
                throw new InvalidOperationException($"{nameof(icon)} ({typeof(Sprite).Name}) ‚Ìæ“¾‚É¸”s‚µ‚Ü‚µ‚½BSerializeField ‚É“o˜^‚µ‚Ä‚­‚¾‚³‚¢B");
        }

        public void OnReload()
        {

        }
    }
}
