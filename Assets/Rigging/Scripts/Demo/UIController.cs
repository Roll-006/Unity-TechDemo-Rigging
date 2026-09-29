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
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private CameraPivotMovement cameraPivotMovement;

        private GameObject _reticle;
        private readonly InventoryViewModel _inventoryViewModel = new();
        private readonly ReticleViewModel _reticleViewModel = new();

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(inventoryView, this);
            Debugger.I.ValidateSerializeField(reticleParent, this);
            Debugger.I.ValidateSerializeField(weaponHolder, this);
            Debugger.I.ValidateSerializeField(shooter, this);
            Debugger.I.ValidateSerializeField(playerMovement, this);
            Debugger.I.ValidateSerializeField(cameraPivotMovement, this);

            inventoryView.SetViewModel(_inventoryViewModel);
            
            _reticleViewModel.ReticleChanged += CreateReticle;
            _reticleViewModel.Initialize(weaponHolder, shooter, playerMovement, cameraPivotMovement);
        }

        private void Update()
        {
            _reticleViewModel.Update();
        }

        private void OnDestroy()
        {
            _inventoryViewModel.Dispose();
            _reticleViewModel.Dispose();
            _reticleViewModel.ReticleChanged -= CreateReticle;
        }

        private void CreateReticle(GameObject reticlePrefab)
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