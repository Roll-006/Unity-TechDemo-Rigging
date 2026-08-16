using UnityEngine;

namespace Rigging.Demo
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
