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
        [SerializeField] private ReticleModel reticleModel;
        [SerializeField, Min(1)] private Vector2Int gridSize;
        [SerializeField] private Sprite icon;

        public override WeaponType WeaponType => WeaponType.Rifle;
        public override WeaponRigData Rig => rig;
        public float InitialVelocity => initialVelocity;
        public float Range => range;
        public Transform AimTarget { get; set; }
        public ReticleModel ReticleModel => reticleModel;
        public ReticleEnlargeState ReticleEnlargeState { private set; get; }
        public Vector2Int GridSize => gridSize;
        public Sprite Icon => icon;

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

        public override void OnFire()
        {

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