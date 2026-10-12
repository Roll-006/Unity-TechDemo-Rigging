using Rigging.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rigging.Demo.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private int gridSize;
        [SerializeField] private Transform grid;
        [SerializeField] private InventoryCell cell;

        private InventoryViewModel _viewModel;
        private readonly List<InventoryCell> _cells = new();

        private void Awake()
        {
            Debugger.ValidateSerializeField(gridLayoutGroup, this);
            Debugger.ValidateSerializeField(grid, this);
            Debugger.ValidateSerializeField(cell, this);
        }

        private void OnEnable()
        {
            _viewModel.ModelChanged += ApplyGridSize;
        }

        private void OnDisable()
        {
            _viewModel.ModelChanged -= ApplyGridSize;
        }

        public void SetViewModel(InventoryViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.Initialize(gridSize, gridLayoutGroup.constraintCount);
            ApplyGridSize(_viewModel.Model);
        }

        private void ApplyGridSize(InventoryModel model)
        {
            gridLayoutGroup.constraintCount = model.FixedColumnCount;
            var count = model.GridSize - _cells.Count;

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    _cells.Add(Instantiate(cell, grid));
                }
            }
            else
            {
                for (int i = 0; i < -count; i++)
                {
                    var cell = _cells[^1];
                    _cells.Remove(cell);
                    Destroy(cell.gameObject);
                }
            }
        }
    }
}