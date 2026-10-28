using Rigging.Common;
using Rigging.Demo.Weapons;
using UnityEngine;

namespace Rigging.Demo.Characters.Player
{
    public class SwayAimUpdater
    {
        private const float kInitialSeed = 100f;
        
        private readonly Vector2 _seed = new(Random.value * kInitialSeed, Random.value * kInitialSeed);
        
        /// <summary>
        /// 照準を揺らす。
        /// </summary>
        public void SwayAim(IShootable shootableWeapon, CameraPivotShareAxis cameraPivotShareAxis, bool isAiming)
        {
            if (!isAiming) return;
            if (shootableWeapon == null) return;

            // -1～1範囲を基盤に SwayAmount 倍する
            var scale = 2f * shootableWeapon.SwayAmount;
            var t = shootableWeapon.SwaySpeed * Time.time;
            cameraPivotShareAxis.SetNoise(new Vector2(Mathf.PerlinNoise(_seed.x, t) - 0.5f, Mathf.PerlinNoise(_seed.y, t) - 0.5f) * scale);
        }
    }
}