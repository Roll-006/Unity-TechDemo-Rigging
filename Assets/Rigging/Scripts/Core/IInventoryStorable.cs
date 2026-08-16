using UnityEngine;

namespace Rigging.Core
{
    public interface IInventoryStorable
    {
        public Vector2Int GridSize { get; }
        public Sprite Icon { get; }
    }
}
