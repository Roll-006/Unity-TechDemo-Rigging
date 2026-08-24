using UnityEngine;

namespace Rigging.Core
{
    public interface IShootable
    {
        public float InitialVelocity { get; }
        public float Range { get; }
        public Transform AimTarget { get; }

        public void SetAimTarget(Transform aimTarget);
        public void UpdateAimTarget();
    }
}
