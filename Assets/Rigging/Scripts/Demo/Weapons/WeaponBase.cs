using Rigging.Demo.Items;

namespace Rigging.Demo.Weapons
{
    public abstract class WeaponBase : ItemBase
    {
        public abstract WeaponType WeaponType { get; }
        public abstract WeaponRigData Rig { get; }
    }
}