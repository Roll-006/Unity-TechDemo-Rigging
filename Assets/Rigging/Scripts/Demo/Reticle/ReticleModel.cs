using System;
using R3;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    [Serializable]
    public class ReticleModel : IDisposable
    {
        /// <summary>
        /// 現在のレティクルサイズ
        /// </summary>
        public ReactiveProperty<float> CurrentSize = new ReactiveProperty<float>();
        
        /// <summary>
        /// レティクルが完全に閉じたかどうか
        /// </summary>
        public ReactiveProperty<bool> IsReticleClosed = new ReactiveProperty<bool>();
        
        /// <summary>
        /// 障害物の座標
        /// </summary>
        public ReactiveProperty<Vector3?> ObstaclePosition = new ReactiveProperty<Vector3?>();
        
        /// <summary>
        /// レティクルの図形の種類
        /// </summary>
        public ReticleShapeType ReticleShapeType;
        
        /// <summary>
        /// レティクルのプレハブ
        /// </summary>
        public GameObject ReticlePrefab;
        
        /// <summary>
        /// 最小サイズ
        /// </summary>
        public float MinSize;
        
        /// <summary>
        /// 中間サイズ
        /// </summary>
        public float MiddleSize;
        
        /// <summary>
        /// 最大サイズ
        /// </summary>
        public float MaxSize;
        
        /// <summary>
        /// 障害物に衝突した際のサイズ
        /// </summary>
        public float ObstacleSize;
        
        /// <summary>
        /// MinSize -> MiddleSize への拡大速度
        /// </summary>
        public float FirstStepEnlargeSpeed;
        
        /// <summary>
        /// MiddleSize -> MinSize への縮小速度
        /// </summary>
        public float FirstStepReduceSpeed;
        
        /// <summary>
        /// MiddleSize -> MaxSize への拡大速度
        /// </summary>
        public float SecondStepEnlargeSpeed;
        
        /// <summary>
        /// MaxSize -> MiddleSize への縮小速度
        /// </summary>
        public float SecondStepReduceSpeed;

        public void Dispose()
        {
            CurrentSize.Dispose();
            IsReticleClosed.Dispose();
            ObstaclePosition.Dispose();
        }
    }
}