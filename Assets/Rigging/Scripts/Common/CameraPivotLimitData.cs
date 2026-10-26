using System;
using UnityEngine;

namespace Rigging.Common
{
    [Serializable]
    public struct CameraPivotLimitData
    {
        [Range(-180f, 180f)] public float MaxYaw;
        [Range(-180f, 180f)] public float MinYaw;
        [Range(-90f, 90f)] public float MaxPitch;
        [Range(-90f, 90f)] public float MinPitch;
        public bool IsWrapYaw;
        public bool IsWrapPitch;
    }
}