using Rigging.Common;
using Rigging.Core;
using Rigging.Demo.Inventory;
using Rigging.Demo.Player;
using Rigging.Demo.Reticle;
using UnityEngine;

namespace Rigging.Demo
{
    // FIX : 責務が大きい
    public class UIController : MonoBehaviour
    {
        [SerializeField] private InventoryView inventoryView;
        [SerializeField] private Transform reticleParent;
        [SerializeField] private WeaponHolder weaponHolder;
        [SerializeField] private Shooter shooter;

        private GameObject _reticle;
        private readonly InventoryViewModel _inventoryViewModel = new();
        private readonly ReticleViewModel _reticleViewModel = new();

        private void Awake()
        {
            Debugger.ValidateSerializeField(inventoryView, this);
            Debugger.ValidateSerializeField(reticleParent, this);
            Debugger.ValidateSerializeField(weaponHolder, this);
            Debugger.ValidateSerializeField(shooter, this);

            inventoryView.SetViewModel(_inventoryViewModel);
            
            _reticleViewModel.ReticleCreateChanged += CreateReticleCreate;
            _reticleViewModel.Initialize(weaponHolder, shooter);
        }

        private void Update()
        {
            _reticleViewModel.Update();
        }

        private void OnDestroy()
        {
            _inventoryViewModel.Dispose();
            _reticleViewModel.ReticleCreateChanged -= CreateReticleCreate;
            _reticleViewModel.Dispose();
        }

        private void CreateReticleCreate(GameObject reticlePrefab)
        {
            if (reticlePrefab != null)
            {
                _reticle = Instantiate(reticlePrefab, reticleParent);
                var view = _reticle.GetComponent<IReticleView>();
                view.SetViewModel(_reticleViewModel);
            }
            else
            {
                Destroy(_reticle);
                _reticle = null;
            }
        }
    }
}