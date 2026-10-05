using System;

namespace Rigging.Demo.Inventory
{
    public class InventoryViewModel : IDisposable
    {
        public InventoryModel Model { get; private set; }

        /// <summary>
        /// Model が変更された
        /// </summary>
        public event Action<InventoryModel> ModelChanged;

        public InventoryViewModel()
        {
            Model = new InventoryModel();
        }

        public void Initialize(int gridSize, int fixedColumnCount)
        {
            Model.GridSize = gridSize;
            Model.FixedColumnCount = fixedColumnCount;
        }

        public void Dispose()
        {

        }

        public void AddGrid(int gridSize, int fixedColumnCount)
        {
            Model.GridSize = gridSize;
            Model.FixedColumnCount = fixedColumnCount;

            ModelChanged?.Invoke(Model);
        }
    }
}
