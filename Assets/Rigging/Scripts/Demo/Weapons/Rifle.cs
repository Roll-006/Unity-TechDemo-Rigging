using Rigging.Core;
using Rigging.Demo.Inventory;
using Rigging.Demo.Items;
using Rigging.Demo.Reticle;
using UnityEngine;

namespace Rigging.Demo.Weapons
{
    public class Rifle : WeaponBase, IShootable, IReloadable, IReticleProvider, IInventoryStorable
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

        public override WeaponType WeaponType => WeaponType.Rifle;
        public override WeaponRigData Rig => rig;
        public float InitialVelocity => initialVelocity;
        public float Range => range;
        public float SwayAmount => swayAmount;
        public float  SwaySpeed => swaySpeed;
        public Transform AimTarget { get; set; }
        public int AmmoAmount => ammoAmount;
        public int MaxAmmoAmount => maxAmmoAmount;
        public ReticleModel ReticleModel => reticleModel;
        public ReticleEnlargeState ReticleEnlargeState { private set; get; }
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

        public void Shoot(GameObjectPool bulletPool, GameObjectPool hitPointPool)
        {
            var bullet = bulletPool.Get().GetComponent<Bullet>();
            bullet?.OnShot(InitialVelocity, Range, rayPivot, _weaponRay.direction, bulletPool);
        }

        public void UpdateAimTarget()
        {
            var outputCamera = CameraManager.I.OutputCamera;
            if (outputCamera == null) return;
            
            // カメラからのレイ
            var cameraTransform = outputCamera.transform;
            var cameraRay = new Ray(cameraTransform.position + cameraTransform.forward * 2f, cameraTransform.forward);
            var obstaclePosition = Physics.Raycast(cameraRay, out var cameraTarget) ? cameraTarget.point : cameraTransform.position + cameraTransform.forward * Range;
            var direction = (obstaclePosition - rayPivot.position).normalized;

            // 武器からのレイ
            _weaponRay = new Ray(rayPivot.position, direction);
            AimTarget.position = Physics.Raycast(_weaponRay, out var weaponTarget) ? weaponTarget.point : obstaclePosition;
            
            // 障害物と衝突した場合座標を通知
            ReticleModel.ObstaclePosition.Value = Vector3.Distance(AimTarget.position, obstaclePosition) > 0.1f ? AimTarget.position : null;
        }

        public void OnReload()
        {

        }
        
        public void OnEnlargeReticle()
        {
            ReticleEnlargeState =  ReticleEnlargeState.Enlarge;
            
            if (ReticleModel.CurrentSize.Value < ReticleModel.MiddleSize)
            {
                ReticleModel.CurrentSize.Value = ReticleModel.MiddleSize;
            }
        }

        public void OnReduceReticle()
        {
            ReticleEnlargeState =  ReticleEnlargeState.Reduce;
        }

        public void OnStopReticle()
        {
            ReticleEnlargeState =  ReticleEnlargeState.Stop;
        }
        
        public void UpdateReticle()
        {
            ReticleUpdater.CalcReticleSize(ReticleModel, ReticleEnlargeState);
        }
    }
}