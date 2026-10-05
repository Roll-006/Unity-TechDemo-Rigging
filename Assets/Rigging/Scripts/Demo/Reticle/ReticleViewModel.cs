using System;
using Rigging.Core;
using Rigging.Demo.Player;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    public class ReticleViewModel : IDisposable
    {
        /// <summary>
        /// レティクル生成イベント
        /// </summary>
        public event Action<GameObject> ReticleCreateChanged;
        
        /// <summary>
        /// レティクルサイズの変更。
        /// </summary>
        public event Action<float> ReticleSizeChanged;
        
        /// <summary>
        /// レティクルアルファ値の変更。
        /// </summary>
        public event Action<float> ReticleAlphaChanged;
        
        private WeaponHolder _weaponHolder;
        private Shooter _shooter;
        private IReticleProvider _reticleProvider;

        public void Initialize(WeaponHolder weaponHolder, Shooter shooter)
        {
            _weaponHolder = weaponHolder;
            _shooter = shooter;
            
            _weaponHolder.EquippedWeaponInstanceChanged += OnInvokeReticle;
            _shooter.AimInputThisFrameChanged += OnOpaqueReticle;
            _shooter.ReleaseInputThisFrameChanged += OnTransparentReticle;
        }

        public void Dispose()
        {
            _weaponHolder.EquippedWeaponInstanceChanged -= OnInvokeReticle;
            _shooter.AimInputThisFrameChanged -= OnOpaqueReticle;
            _shooter.ReleaseInputThisFrameChanged -= OnTransparentReticle;
        }

        public void Update()
        {
            if (_reticleProvider == null) return;
            ReticleSizeChanged?.Invoke(_reticleProvider.CurrentReticleSize);
        }

        /// <summary>
        /// レティクルの生成を通知する。
        /// </summary>
        /// <param name="equippedWeaponInstance">Hierarchy 上に実在する装備している武器</param>
        private void OnInvokeReticle(WeaponBase equippedWeaponInstance)
        {
            if (equippedWeaponInstance == null)
            {
                _reticleProvider = null;
                ReticleCreateChanged?.Invoke(null);
                return;
            }

            _reticleProvider = equippedWeaponInstance.GetComponent<IReticleProvider>();
            if (_reticleProvider == null) return;

            ReticleCreateChanged?.Invoke(_reticleProvider.ReticleData.ReticlePrefab);
        }

        /// <summary>
        /// 不透明にする。
        /// </summary>
        private void OnOpaqueReticle()
        {
            ReticleAlphaChanged?.Invoke(1f);
        }
        
        /// <summary>
        /// 透過させる。
        /// </summary>
        private void OnTransparentReticle()
        {
            ReticleAlphaChanged?.Invoke(0f);
        }
    }
}
