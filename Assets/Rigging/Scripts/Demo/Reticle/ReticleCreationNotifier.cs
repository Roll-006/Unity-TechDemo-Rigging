using R3;
using Rigging.Core;
using Rigging.Demo.Characters.Player;
using Rigging.Demo.Weapons;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    /// <summary>
    /// レティクルの生成を通知する。
    /// </summary>
    public class ReticleCreationNotifier : MonoBehaviour
    {
        [SerializeField] private WeaponHolder weaponHolder;
        
        /// <summary>
        /// レティクルの生成通知
        /// </summary>
        public ReadOnlyReactiveProperty<ReticleModel> CreateReticle => _createReticle;
        
        private readonly ReactiveProperty<ReticleModel> _createReticle = new ReactiveProperty<ReticleModel>();
        private IReticleProvider _reticleProvider;
        
        private void Awake()
        {
            Debugger.ValidateSerializeField(weaponHolder, this);
        }

        private void Start()
        {
            weaponHolder.EquippedWeaponInstanceChanged
                .Skip(1)
                .Where(x => x != null)
                .Subscribe(OnNotifyCreateReticle)
                .AddTo(this);
        }

        private void OnDestroy()
        {
            _createReticle.Dispose();
        }
        
        /// <summary>
        /// レティクルの生成を通知する。
        /// </summary>
        /// <param name="equippedWeaponInstance">Hierarchy 上に実在する装備している武器</param>
        private void OnNotifyCreateReticle(WeaponBase equippedWeaponInstance)
        {
            _reticleProvider = equippedWeaponInstance.GetComponent<IReticleProvider>();
            if (_reticleProvider == null) return;
            
            _createReticle.Value = _reticleProvider.ReticleModel;
        }
    }
}