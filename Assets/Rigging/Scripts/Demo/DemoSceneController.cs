using Rigging.Core;
using Rigging.Demo.Inventory;
using UnityEngine;

namespace Rigging.Demo
{
    public class DemoSceneController : MonoBehaviour
    {
        [SerializeField] private InventoryView inventoryView;

        private InventoryViewModel _inventoryViewModel = new();

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(inventoryView, this);

            inventoryView.SetViewModel(_inventoryViewModel);
        }

        private void OnDestroy()
        {
            _inventoryViewModel.Dispose();
        }
    }
}
