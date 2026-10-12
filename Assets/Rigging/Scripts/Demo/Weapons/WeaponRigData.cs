using System;
using UnityEngine;

namespace Rigging.Demo.Weapons
{
    [Serializable]
    public struct WeaponRigData
    {
        public Vector3 PivotOffset;
        public Vector3 LeftHandRigHintPosition;
        public Vector3 LeftHandRigTargetPosition;
        public Vector3 LeftHandRigTargetRotation;
    }
}