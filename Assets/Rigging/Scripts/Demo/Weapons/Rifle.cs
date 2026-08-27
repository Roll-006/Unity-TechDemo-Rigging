using Rigging.Core;
using System;
using UnityEngine;

namespace Rigging.Demo.Weapons
{
    public class Rifle : WeaponBase, IInventoryStorable, IReloadable, IShootable, ITriggerable
    {
        [SerializeField] private WeaponRigData rig;
        [SerializeField, Min(1)] private Vector2Int gridSize;
        [SerializeField] private Sprite icon;
        [SerializeField] private float initialVelocity;
        [SerializeField] private float range;
        [SerializeField] private Transform rayPivot;
        [SerializeField] private RayPivotTrigger rayPivotTrigger;

        public override WeaponType WeaponType => WeaponType.Rifle;
        public override WeaponRigData Rig => rig;
        public Vector2Int GridSize => gridSize;
        public Sprite Icon => icon;
        public float InitialVelocity => initialVelocity;
        public float Range => range;
        public Transform AimTarget { get; set; }
        public RayPivotTrigger RayPivotTrigger => rayPivotTrigger;

        private void Awake()
        {
            if (icon == null)
                throw new InvalidOperationException($"{nameof(icon)} ({typeof(Sprite).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (rayPivot == null)
                throw new InvalidOperationException($"{nameof(rayPivot)} ({typeof(Transform).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (rayPivotTrigger == null)
                throw new InvalidOperationException($"{nameof(rayPivotTrigger)} ({typeof(RayPivotTrigger).Name}) の取得に失敗しました。SerializeField に登録してください。");
        }

        private void Update()
        {
            UpdateAimTarget();
        }

        public override void OnFire()
        {

        }

        public void OnReload()
        {

        }

        public void UpdateAimTarget()
        {
            // カメラからのレイ
            var cameraTransform = CameraManager.I.OutputCamera.transform;
            var cameraRay = new Ray(cameraTransform.position + cameraTransform.forward * 2f, cameraTransform.forward);
            var obstaclePosition = Physics.Raycast(cameraRay, out var cameraTarget) ? cameraTarget.point : cameraTransform.position + cameraTransform.forward * Range;

            var distance = Vector3.Distance(rayPivot.position, obstaclePosition);
            var direction = (obstaclePosition - rayPivot.position).normalized;

            // 武器からのレイ
            var weaponRay = new Ray(rayPivot.position, direction);
            AimTarget.position = Physics.Raycast(weaponRay, out var weaponTarget) ? weaponTarget.point : obstaclePosition;
        }
    }
}
