using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Weapons
{
    /// <summary>
    /// 撃つことが可能なオブジェクト
    /// </summary>
    public interface IShootable
    {
        /// <summary>
        /// 初速度
        /// </summary>
        public float InitialVelocity { get; }
        
        /// <summary>
        /// 射程
        /// </summary>
        public float Range { get; }
        
        /// <summary>
        /// 揺れの量
        /// </summary>
        public float SwayAmount { get; }
        
        /// <summary>
        /// 揺れの速度
        /// </summary>
        public float SwaySpeed { get; }
        
        public Transform AimTarget { get; set; }
        
        /// <summary>
        /// 撃つ。
        /// </summary>
        /// <param name="bulletPool">弾丸のプールマネージャー</param>
        public void Shoot(GameObjectPool bulletPool, GameObjectPool hitPointPool);
        
        /// <summary>
        /// 狙う場所を更新する。
        /// </summary>
        public void UpdateAimTarget();
    }
}