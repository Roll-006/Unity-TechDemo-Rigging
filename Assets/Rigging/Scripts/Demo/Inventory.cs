using System;
using UnityEngine;
using System.Collections.Generic;

namespace Rigging.Demo
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private ItemHolder itemHolder;
        [SerializeField, Min(1)] private Vector2Int gridSize;
        [SerializeField] private Transform cellParent;
        [SerializeField] private GameObject cell;

        private List<List<InventoryCell>> cells = new();

        private void Awake()
        {
            if (itemHolder == null)
                throw new InvalidOperationException($"{nameof(itemHolder)} ({typeof(ItemHolder).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (cellParent == null)
                throw new InvalidOperationException($"{nameof(cellParent)} ({typeof(Transform).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (cell == null)
                throw new InvalidOperationException($"{nameof(cell)} ({typeof(GameObject).Name}) の取得に失敗しました。SerializeField に登録してください。");

            CreateGrid();
            AddGrid(new Vector2Int(3, 3));
        }

        private void CreateGrid()
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                List<InventoryCell> row;

                if (i < cells.Count)
                {
                    row = cells[i];
                }
                else
                {
                    row = new List<InventoryCell>();
                    cells.Add(row);
                }

                var columnCreateCount = gridSize.y - row.Count;

                for (int j = 0; j < columnCreateCount; j++)
                {
                    var cell = Instantiate(this.cell, cellParent).GetComponent<InventoryCell>();
                    row.Add(cell);
                }
            }

            AlignCells();
        }

        private void AlignCells()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                for (int j = 0; j < cells[i].Count; j++)
                {
                    var rect = cells[i][j].GetComponent<RectTransform>();

                    var x =
                        -rect.sizeDelta.x * cells.Count * 0.5f +
                        rect.sizeDelta.x * 0.5f +
                        rect.sizeDelta.x * i;

                    var y =
                        1080f * 0.5f -
                        rect.sizeDelta.y * 0.5f -
                        rect.sizeDelta.y * j;

                    rect.anchoredPosition = new Vector2(x, y);
                }
            }
        }

        public void AddGrid(Vector2Int addGridSize)
        {
            gridSize += addGridSize;
            CreateGrid();
        }
    }
}
