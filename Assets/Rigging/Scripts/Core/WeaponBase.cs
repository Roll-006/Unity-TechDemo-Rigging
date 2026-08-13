namespace Rigging.Core
{
    public abstract class WeaponBase : ItemBase
    {
        public abstract WeaponType WeaponType { get; }

        public void OnFire()
        {

        }
    }
}
