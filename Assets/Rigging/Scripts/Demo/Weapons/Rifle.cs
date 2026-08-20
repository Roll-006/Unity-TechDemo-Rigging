using Rigging.Core;
using System;
using UnityEngine;

namespace Rigging.Demo.Weapons
{
    public class Rifle : WeaponBase, IInventoryStorable, IReloadable, IShootable
    {
        [SerializeField] private WeaponRigData rig;
        [SerializeField, Min(1)] private Vector2Int gridSize;
        [SerializeField] private Sprite icon;
        [SerializeField] private float initialVelocity;
        [SerializeField] private float range;
        [SerializeField] private Transform aimTarget;

        public override WeaponType WeaponType => WeaponType.Rifle;
        public override WeaponRigData Rig => rig;
        public Vector2Int GridSize => gridSize;
        public Sprite Icon => icon;
        public float InitialVelocity => initialVelocity;
        public float Range => range;
        public Transform AimTarget { get; }

        private void Awake()
        {
            if (icon == null)
                throw new InvalidOperationException($"{nameof(icon)} ({typeof(Sprite).Name}) の取得に失敗しました。SerializeField に登録してください。");
        }

        public override void OnFire()
        {

        }

        public void OnReload()
        {

        }

        public void UpdateAimTarget()
        {

        }
    }
}
