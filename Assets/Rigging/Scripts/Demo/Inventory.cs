using Rigging.Core;
using UnityEngine;
using System.Collections.Generic;

namespace Rigging.Demo
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<ItemBase> items;

        public List<ItemBase> Items => items;
    }
}
