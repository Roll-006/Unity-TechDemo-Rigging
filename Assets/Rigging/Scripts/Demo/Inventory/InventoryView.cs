using System;
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
        private List<InventoryCell> _cells = new();

        private void Awake()
        {
            if (gridLayoutGroup == null)
                throw new InvalidOperationException($"{nameof(gridLayoutGroup)} ({typeof(GridLayoutGroup).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (grid == null)
                throw new InvalidOperationException($"{nameof(grid)} ({typeof(Transform).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (cell == null)
                throw new InvalidOperationException($"{nameof(cell)} ({typeof(InventoryCell).Name}) の取得に失敗しました。SerializeField に登録してください。");
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

            _viewModel.Model.GridSize = gridSize;
            _viewModel.Model.FixedColumnCount = gridLayoutGroup.constraintCount;

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
