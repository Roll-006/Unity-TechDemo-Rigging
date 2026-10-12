using UnityEngine;

namespace Rigging.Demo.Reticle
{
    public static class ReticleUpdater
    {
        /// <summary>
        /// レティクルのサイズを計算する。
        /// </summary>
        /// <param name="model">レティクルモデル</param>
        /// <param name="reticleEnlargeState">レティクルの拡大状態</param>
        public static void CalcReticleSize(ReticleModel model, ReticleEnlargeState reticleEnlargeState)
        {
            if (reticleEnlargeState == ReticleEnlargeState.Stop) return;

            var isEnlarge = reticleEnlargeState == ReticleEnlargeState.Enlarge;
            
            if (model.CurrentSize.CurrentValue >= model.MaxSize && isEnlarge) return;
            if (model.CurrentSize.CurrentValue <= model.MinSize && !isEnlarge) return;
        
            var isSecondStep = isEnlarge
                ? model.CurrentSize.CurrentValue >= model.MiddleSize
                : model.CurrentSize.CurrentValue > model.MiddleSize;

            var target = isSecondStep
                ? (isEnlarge ? model.MaxSize : model.MiddleSize)
                : (isEnlarge ? model.MiddleSize : model.MinSize);

            var speed = isSecondStep
                ? (isEnlarge ? model.SecondStepEnlargeSpeed : model.SecondStepReduceSpeed)
                : (isEnlarge ? model.FirstStepEnlargeSpeed : model.FirstStepReduceSpeed);

            model.CurrentSize.Value = Mathf.MoveTowards(model.CurrentSize.CurrentValue, target, speed * Time.deltaTime);

            model.IsReticleClosed.Value = model.CurrentSize.Value - model.MinSize <= Mathf.Epsilon;
        }
    }
}