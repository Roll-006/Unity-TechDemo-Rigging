using UnityEngine;

namespace Rigging.Core
{
    /// <summary>
    /// 撃つことが可能なオブジェクト
    /// </summary>
    public interface IShootable
    {
        public float InitialVelocity { get; }
        public float Range { get; }
        public Transform AimTarget { get; set; }
        
        /// <summary>
        /// 狙う場所を更新する。
        /// </summary>
        public void UpdateAimTarget();
    }
}
