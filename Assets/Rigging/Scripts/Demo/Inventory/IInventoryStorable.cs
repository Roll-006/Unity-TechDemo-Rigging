using UnityEngine;

namespace Rigging.Demo.Inventory
{
    public interface IInventoryStorable
    {
        public Vector2Int GridSize { get; }
        public Sprite Icon { get; }
    }
}