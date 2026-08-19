using Rigging.Core;
using System;
using UnityEngine;

namespace Rigging.Demo.Weapons
{
    public class Rifle : WeaponBase, IInventoryStorable, IReloadable
    {
        [SerializeField] private WeaponRigData rig;
        [SerializeField, Min(1)] private Vector2Int gridSize;
        [SerializeField] private Sprite icon;

        public override WeaponType WeaponType => WeaponType.Rifle;
        public override WeaponRigData Rig => rig;
        public Vector2Int GridSize => gridSize;
        public Sprite Icon => icon;

        private void Awake()
        {
            if (icon == null)
                throw new InvalidOperationException($"{nameof(icon)} ({typeof(Sprite).Name}) の取得に失敗しました。SerializeField に登録してください。");
        }

        public void OnReload()
        {

        }
    }
}
