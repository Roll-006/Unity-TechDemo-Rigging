using R3;
using Rigging.Core;
using Rigging.Demo.Player;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    public class ReticleLifecycleNotifier : MonoBehaviour
    {
        [SerializeField] private WeaponHolder weaponHolder;
        
        /// <summary>
        /// レティクルライフサイクル通知 (T: ReticleModel)
        /// </summary>
        public ReadOnlyReactiveProperty<ReticleModel> ReticleLifecycle => _reticleLifecycle;
        
        private readonly ReactiveProperty<ReticleModel> _reticleLifecycle = new ReactiveProperty<ReticleModel>();
        private IReticleProvider _reticleProvider;
        private GameObject _previousReticlePrefab;
        
        private void Awake()
        {
            Debugger.ValidateSerializeField(weaponHolder, this);
        }

        private void Start()
        {
            weaponHolder.EquippedWeaponInstanceChanged
                .Skip(1)
                .Where(x => x != null)
                .Subscribe(OnCreateReticle)
                .AddTo(this);
        }

        private void OnDestroy()
        {
            _reticleLifecycle.Dispose();
        }
        
        /// <summary>
        /// レティクルの生成を通知する。
        /// </summary>
        /// <param name="equippedWeaponInstance">Hierarchy 上に実在する装備している武器</param>
        private void OnCreateReticle(WeaponBase equippedWeaponInstance)
        {
            _reticleProvider = equippedWeaponInstance.GetComponent<IReticleProvider>();
            if (_reticleProvider == null) return;
            
            // 以前再生されていたレティクルと同じ場合は再生成しない
            if (_previousReticlePrefab == _reticleProvider.ReticleModel.ReticlePrefab) return;
            
            _previousReticlePrefab = _reticleProvider.ReticleModel.ReticlePrefab;
            _reticleLifecycle.Value = _reticleProvider.ReticleModel;
        }
    }
}