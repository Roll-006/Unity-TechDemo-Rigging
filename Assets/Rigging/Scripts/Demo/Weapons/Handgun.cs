using System.Collections.Generic;
using Rigging.Core;
using Rigging.Demo.Inventory;
using Rigging.Demo.Items;
using Rigging.Demo.Reticle;
using UnityEngine;

namespace Rigging.Demo.Weapons
{
    public class Handgun : WeaponBase, IShootable, IReloadable, IReticleProvider, IOwnable, IInventoryStorable
    {
        [SerializeField] private WeaponRigData rig;
        [SerializeField] private float initialVelocity;
        [SerializeField] private float range;
        [SerializeField, Min(0)] private float swayAmount;
        [SerializeField, Min(0)] private float swaySpeed;
        [SerializeField] private Transform rayPivot;
        [SerializeField] private int ammoAmount;
        [SerializeField] private int maxAmmoAmount;
        [SerializeField] private ReticleModel reticleModel;
        [SerializeField, Min(1)] private Vector2Int gridSize;
        [SerializeField] private Sprite icon;

        public override WeaponType WeaponType => WeaponType.Handgun;
        public override WeaponRigData Rig => rig;
        public float InitialVelocity => initialVelocity;
        public float Range => range;
        public float SwayAmount => swayAmount;
        public float SwaySpeed => swaySpeed;
        public Transform AimTarget { get; set; }
        public int AmmoAmount => ammoAmount;
        public int MaxAmmoAmount => maxAmmoAmount;
        public ReticleModel ReticleModel => reticleModel;
        public ReticleEnlargeState ReticleEnlargeState { private set; get; }
        public GameObject Owner { get; private set; }
        public Vector2Int GridSize => gridSize;
        public Sprite Icon => icon;

        private Ray _weaponRay;

        private void Awake()
        {
            Debugger.ValidateSerializeField(icon, this);
            Debugger.ValidateSerializeField(rayPivot, this);
        }

        private void Start()
        {
            ReticleModel.CurrentSize.Value = ReticleModel.MaxSize;
        }

        private void Update()
        {
            UpdateReticle();
            UpdateAimTarget();
        }

        public void Shoot(GameObjectPool bulletPool, RectTransform rootRectTransform, GameObjectPool hitPointPool, List<Collider> ignoreColliders)
        {
            var obj = bulletPool.Get();
            
            var bullet = obj.GetComponent<Bullet>();
            if (bullet == null) return;
            var ownable = obj.GetComponent<IOwnable>();
            if (ownable == null) return;
            
            bullet.OnShot(InitialVelocity, Range, rayPivot, ShootDirectionCalculator.CalcShootDirection(ReticleModel, AimTarget.position, rootRectTransform, _weaponRay), bulletPool, ignoreColliders);
            ownable.SetOwner(Owner);
            OnEnlargeReticle();
            
            bullet.RecordHitPoint(hitPointPool);
        }

        public void UpdateAimTarget()
        {
            var outputCamera = CameraManager.I.OutputCamera;
            if (outputCamera == null) return;
            
            // カメラからのレイ
            var cameraTransform = outputCamera.transform;
            var cameraRay = new Ray(cameraTransform.position + cameraTransform.forward * 2f, cameraTransform.forward);
            var cameraTarget = Physics.Raycast(cameraRay, out var cameraHitInfo) ? cameraHitInfo.point : cameraTransform.position + cameraTransform.forward * Range;

            // 武器からのレイ
            var direction = (cameraTarget - rayPivot.position).normalized;
            _weaponRay = new Ray(rayPivot.position, direction);
            if (Physics.Raycast(_weaponRay, out var weaponHitInfo))
            {
                AimTarget.position = !weaponHitInfo.collider.TryGetComponent<IOwnable>(out var ownable) || ownable.Owner != Owner ? 
                    weaponHitInfo.point : cameraTarget;
            }
            else
            {
                AimTarget.position = cameraTarget;
            }

            // 障害物と衝突した場合座標を通知
            ReticleModel.ObstaclePosition.Value = Vector3.Distance(AimTarget.position, cameraTarget) > 0.1f && Vector3.Distance(AimTarget.position, _weaponRay.origin) < 4f ? AimTarget.position : null;
        }

        public void OnReload()
        {

        }

        public void SetEnlargeState(ReticleEnlargeState state)
        {
            ReticleEnlargeState = state;
        }

        public void OnEnlargeReticle()
        {
            ReticleModel.CurrentSize.Value = Mathf.Min(ReticleModel.MaxSize, ReticleModel.CurrentSize.CurrentValue + ReticleModel.ShotEnlargeAmount);
        }

        public void UpdateReticle()
        {
            ReticleUpdater.CalcReticleSize(ReticleModel, ReticleEnlargeState);
        }

        public void SetOwner(GameObject owner)
        {
            Owner = owner;
        }
    }
}