using System;
using UnityEngine;
using System.Collections.Generic;

namespace Rigging.Demo
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private ItemHolder itemHolder;
        [SerializeField, Min(1)] private Vector2Int gridSize;
        [SerializeField] private GameObject cell;

        private List<List<InventoryCell>> cells = new();

        private void Awake()
        {
            if (itemHolder == null)
                throw new InvalidOperationException($"{nameof(itemHolder)} ({typeof(ItemHolder).Name}) の取得に失敗しました。SerializeField に登録してください。");

            if (cell == null)
                throw new InvalidOperationException($"{nameof(cell)} ({typeof(GameObject).Name}) の取得に失敗しました。SerializeField に登録してください。");

            CreateGrid();
            //AddGrid(new Vector2Int(3, 3));
        }

        private void CreateGrid()
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                var row = i < cells.Count ? cells[i] : new List<InventoryCell>();

                for (int j = gridSize.y - 1 ; j >= 0 ; j--)
                {
                    var cell = Instantiate(this.cell).GetComponent<InventoryCell>();

                    cell.gameObject.transform.SetParent(transform);
                    row.Add(cell);
                }

                cells.Add(row);
            }

            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = gridSize.y - 1; j >= 0; j--)
                {
                    var rect = cells[i][j].GetComponent<RectTransform>();

                    var x =
                        1920f * 0.5f -
                        rect.sizeDelta.x * gridSize.x * 0.5f +
                        rect.sizeDelta.x * 0.5f +
                        rect.sizeDelta.x * i;

                    var y =
                        rect.sizeDelta.y * 0.5f +
                        rect.sizeDelta.y * j;

                    rect.anchoredPosition = new Vector2(x, y);
                }
            }
        }

        private void AddGrid(Vector2Int addGridSize)
        {
            gridSize += addGridSize;
            CreateGrid();
        }
    }
}
