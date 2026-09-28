using Rigging.Demo.Reticle;
using UnityEngine;

namespace Rigging.Core
{
    public interface IShootable
    {
        public ReticleData ReticleData { get; }
        public float InitialVelocity { get; }
        public float Range { get; }
        public Transform AimTarget { get; set; }

        public void UpdateAimTarget();
    }
}
