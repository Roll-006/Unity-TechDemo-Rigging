using Rigging.Core;

namespace Rigging.Common
{
    public class Handgun : WeaponBase, IReloadable
    {
        public override WeaponType WeaponType => WeaponType.Handgun;

        public void OnReload()
        {

        }
    }
}
