using UnityEngine;

namespace Rigging.Common
{
    public static class Layers
    {
        public static readonly int Default = LayerMask.NameToLayer("Default");
        public static readonly int TransparentFX = LayerMask.NameToLayer("TransparentFX");
        public static readonly int IgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        public static readonly int Player = LayerMask.NameToLayer("Player");
        public static readonly int Water = LayerMask.NameToLayer("Water");
        public static readonly int UI = LayerMask.NameToLayer("UI");
        public static readonly int Walkable = LayerMask.NameToLayer("Walkable");
    }
}