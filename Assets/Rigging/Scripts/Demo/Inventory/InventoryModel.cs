using Rigging.Core;
using System.Collections.Generic;

namespace Rigging.Demo.Inventory
{
    public class InventoryModel
    {
        public List<IInventoryStorable> Items = new();
        public int GridSize;
        public int FixedColumnCount;

        public void AddItem(IInventoryStorable item)
        {
            Items.Add(item);
        }

        public void AddGrid(int gridSize, int fixedColumnCount)
        {
            GridSize = gridSize;
            FixedColumnCount = fixedColumnCount;
        }
    }
}
