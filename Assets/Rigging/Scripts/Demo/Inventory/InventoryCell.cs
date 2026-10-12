using UnityEngine;

namespace Rigging.Demo.Inventory
{
    public class InventoryCell : MonoBehaviour
    {
        public bool IsUsing {  get; private set; }

        public void OnUse()
        {
            IsUsing = true;
        }
    }
}