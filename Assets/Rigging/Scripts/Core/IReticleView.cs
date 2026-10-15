using UnityEngine;

namespace Rigging.Core
{
    public interface IReticleView
    {
        public void Initialize(RectTransform rootRectTransform);
        
        /// <summary>
        /// レティクルの表示サイズを設定する。
        /// </summary>
        /// <param name="size">レティクルサイズ</param>
        public void SetReticleSize(float size);
        
        /// <summary>
        /// 障害物用レティクルの表示サイズを設定する。
        /// </summary>
        /// <param name="size"></param>
        public void SetObstacleReticleSize(float size);
        
        /// <summary>
        /// メインレティクルのターゲットアルファ値を設定する。
        /// </summary>
        /// <param name="alpha">アルファ値</param>
        public void SetTargetAlpha(float alpha);
        
        /// <summary>
        /// レティクルの中心ポイントの有効状態を設定する。
        /// </summary>
        /// <param name="isReticleClosed">レティクルが閉じたかどうか</param>
        public void EnableCenterPoint(bool isReticleClosed);
        
        /// <summary>
        /// 障害物用レティクルの使用を許可するかを設定する。
        /// </summary>
        /// <param name="isAiming">エイム状態かどうか</param>
        public void AllowToUseObstacleReticle(bool isAiming);
        
        /// <summary>
        /// 障害物用レティクルを非アクティブ化する。
        /// </summary>
        /// <param name="isAiming">非アクティブ化するかどうか</param>
        public void DeactivateObstacleReticle(bool isAiming);
        
        /// <summary>
        /// 障害物用レティクルに座標を設定する。
        /// </summary>
        /// <param name="obstaclePosition">障害物と衝突した座標</param>
        public void ActivateObstacleReticle(Vector3? obstaclePosition);

        public void UpdateAlpha();
    }
}