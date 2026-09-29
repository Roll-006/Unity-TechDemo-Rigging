using System;
using Rigging.Common;
using Rigging.Core;
using Rigging.Demo.Player;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    public class ReticleViewModel : IDisposable
    {
        public event Action<GameObject> ReticleChanged;
        public event Action<float> ReticleSizeChanged;
        public event Action<float> ReticleAlphaChanged;
        
        private WeaponHolder _weaponHolder;
        private Shooter _shooter;
        private PlayerMovement _playerMovement;
        private CameraPivotMovement _cameraPivotMovement;
        private IReticleProvider _reticleProvider;

        private void OnWeaponInstanceChanged(WeaponBase equippedWeaponInstance)
        {
            if (equippedWeaponInstance == null)
            {
                _reticleProvider = null;
                ReticleChanged?.Invoke(null);
                return;
            }

            _reticleProvider = equippedWeaponInstance.GetComponent<IReticleProvider>();

            if (_reticleProvider == null) return;

            ReticleChanged?.Invoke(_reticleProvider.ReticleData.ReticlePrefab);
        }

        public void Initialize(WeaponHolder weaponHolder, Shooter shooter, PlayerMovement playerMovement, CameraPivotMovement cameraPivotMovement)
        {
            _weaponHolder = weaponHolder;
            _shooter = shooter;
            _playerMovement = playerMovement;
            _cameraPivotMovement = cameraPivotMovement;
            _weaponHolder.EquippedWeaponInstanceChanged += OnWeaponInstanceChanged;
        }

        public void Dispose()
        {
            _weaponHolder.EquippedWeaponInstanceChanged -= OnWeaponInstanceChanged;
        }

        public void Update()
        {
            if (_reticleProvider == null) return;
            
            ReticleSizeChanged?.Invoke(_reticleProvider.CurrentReticleSize);
            StartReticle();
        }
        
        private void StartReticle()
        {
            if (!_shooter.IsAiming) return;
            if (_shooter.WasAiming) return;
            
            ReticleAlphaChanged?.Invoke(1f);
        }
    }
}