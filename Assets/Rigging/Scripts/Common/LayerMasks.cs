using UnityEngine;

namespace Rigging.Common
{
    public static class LayerMasks
    {
        public static readonly int Default = LayerMask.GetMask("Default");
        public static readonly int TransparentFX = LayerMask.GetMask("TransparentFX");
        public static readonly int IgnoreRaycast = LayerMask.GetMask("Ignore Raycast");
        public static readonly int Player = LayerMask.GetMask("Player");
        public static readonly int Water = LayerMask.GetMask("Water");
        public static readonly int UI = LayerMask.GetMask("UI");
        public static readonly int Walkable = LayerMask.GetMask("Walkable");
    }
}