using UnityEngine;

namespace Rigging.Core
{
    public abstract class WeaponBase : ItemBase
    {
        public abstract WeaponType WeaponType { get; }
        public abstract Vector3 PivotOffset { get; }


        public void OnFire()
        {

        }
    }
}
