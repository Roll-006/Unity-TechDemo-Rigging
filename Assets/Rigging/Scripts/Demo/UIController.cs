using Rigging.Core;
using Rigging.Demo.Inventory;
using UnityEngine;

namespace Rigging.Demo
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private InventoryView inventoryView;

        private readonly InventoryViewModel _inventoryViewModel = new();

        private void Awake()
        {
            Debugger.ValidateSerializeField(inventoryView, this);

            inventoryView.SetViewModel(_inventoryViewModel);
        }

        private void OnDestroy()
        {
            _inventoryViewModel.Dispose();
        }
    }
}