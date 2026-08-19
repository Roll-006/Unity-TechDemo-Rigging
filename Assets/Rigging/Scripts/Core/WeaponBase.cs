using UnityEngine;

namespace Rigging.Core
{
    public abstract class WeaponBase : ItemBase
    {
        public abstract WeaponType WeaponType { get; }
        public abstract WeaponRigData Rig { get; }

        public void OnFire()
        {

        }
    }
}
