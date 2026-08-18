using Rigging.Demo.Inventory;
using Rigging.Demo.Player;
using System;
using UnityEngine;

namespace Rigging.Demo
{
    public class DemoSceneController : MonoBehaviour
    {
        [SerializeField] private InventoryView inventoryView;

        private InventoryViewModel inventoryViewModel = new();

        private void Awake()
        {
            if (inventoryView == null)
                throw new InvalidOperationException($"{nameof(inventoryView)} ({typeof(InventoryView).Name}) の取得に失敗しました。SerializeField に登録してください。");

            inventoryView.SetViewModel(inventoryViewModel);
        }

        private void OnDestroy()
        {
            inventoryViewModel.Dispose();
        }
    }
}
