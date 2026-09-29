using Rigging.Core;
using Rigging.Demo.Reticle;
using UnityEngine;

namespace Rigging.Demo.Weapons
{
    public class Rifle : WeaponBase, IShootable, IReloadable, IReticleProvider, IInventoryStorable
    {
        [SerializeField] private WeaponRigData rig;
        [SerializeField] private float initialVelocity;
        [SerializeField] private float range;
        [SerializeField] private Transform rayPivot;
        [SerializeField] private ReticleData reticleData;
        [SerializeField, Min(1)] private Vector2Int gridSize;
        [SerializeField] private Sprite icon;

        public override WeaponType WeaponType => WeaponType.Rifle;
        public override WeaponRigData Rig => rig;
        public float InitialVelocity => initialVelocity;
        public float Range => range;
        public Transform AimTarget { get; set; }
        public ReticleData ReticleData => reticleData;
        public float CurrentReticleSize { get; private set; }
        public bool IsEnlarge { get; private set; }
        public Vector2Int GridSize => gridSize;
        public Sprite Icon => icon;

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(icon, this);
            Debugger.I.ValidateSerializeField(rayPivot, this);
            
            CurrentReticleSize = ReticleData.MaxSize;
        }

        private void Update()
        {
            UpdateReticle();
            UpdateAimTarget();
        }

        public override void OnFire()
        {

        }

        public void OnReload()
        {

        }
        
        public void OnEnlargeReticle()
        {
            IsEnlarge = true;
        }

        public void OnReduceReticle()
        {
            IsEnlarge = false;
        }
        
        public void UpdateReticle()
        {
            CurrentReticleSize = ReticleUpdater.CalcReticleSize(CurrentReticleSize, reticleData, IsEnlarge);
        }

        public void UpdateAimTarget()
        {
            // カメラからのレイ
            var cameraTransform = CameraManager.I.OutputCamera.transform;
            var cameraRay = new Ray(cameraTransform.position + cameraTransform.forward * 2f, cameraTransform.forward);
            var obstaclePosition = Physics.Raycast(cameraRay, out var cameraTarget) ? cameraTarget.point : cameraTransform.position + cameraTransform.forward * Range;
            var direction = (obstaclePosition - rayPivot.position).normalized;

            // 武器からのレイ
            var weaponRay = new Ray(rayPivot.position, direction);
            AimTarget.position = Physics.Raycast(weaponRay, out var weaponTarget) ? weaponTarget.point : obstaclePosition;
        }
    }
}
