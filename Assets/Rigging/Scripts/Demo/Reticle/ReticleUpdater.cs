using UnityEngine;

namespace Rigging.Demo.Reticle
{
    public static class ReticleUpdater
    {
        /// <summary>
        /// レティクルのサイズを計算する。
        /// </summary>
        /// <param name="currentSize">現在のレティクルサイズ</param>
        /// <param name="data">レティクルデータ</param>
        /// <param name="isEnlarge">拡大するかどうか</param>
        /// <returns>レティクルサイズ</returns>
        public static float CalcReticleSize(float currentSize, ReticleData data, bool isEnlarge)
        {
            if (currentSize >= data.MaxSize && isEnlarge) return data.MaxSize;
            if (currentSize <= data.MinSize && !isEnlarge) return data.MinSize;
        
            var isSecondStep = isEnlarge
                ? currentSize >= data.MiddleSize
                : currentSize > data.MiddleSize;

            var target = isSecondStep
                ? (isEnlarge ? data.MaxSize : data.MiddleSize)
                : (isEnlarge ? data.MiddleSize : data.MinSize);

            var speed = isSecondStep
                ? (isEnlarge ? data.SecondStepEnlargeSpeed : data.SecondStepReduceSpeed)
                : (isEnlarge ? data.FirstStepEnlargeSpeed : data.FirstStepReduceSpeed);

            return Mathf.MoveTowards(currentSize, target, speed * Time.deltaTime);
        }
        
        public static void UpdateReticle(ReticleData data)
        {
            switch (data.ShapeType)
            {
                case ReticleShapeType.Square:
                    UpdateSquareReticle(data);
                    break;
                
                case ReticleShapeType.Circle:
                    UpdateCircleReticle(data);
                    break;
                
                case ReticleShapeType.Capsule:
                    UpdateCapsuleReticle(data);
                    break;
            }
        }
        
        private static void UpdateSquareReticle(ReticleData data)
        {
            
        }

        private static void UpdateCircleReticle(ReticleData data)
        {
            
        }
        
        private static void UpdateCapsuleReticle(ReticleData data)
        {
            
        }
    }
}