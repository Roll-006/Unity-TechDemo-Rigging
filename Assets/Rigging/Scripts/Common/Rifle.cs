using Rigging.Core;

namespace Rigging.Common
{
    public class Rifle : WeaponBase, IReloadable
    {
        public override WeaponType WeaponType => WeaponType.Rifle;

        public void OnReload()
        {

        }
    }
}
