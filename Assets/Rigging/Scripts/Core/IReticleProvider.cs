using Rigging.Demo.Reticle;

namespace Rigging.Core
{
    /// <summary>
    /// レティクルを持つオブジェクト
    /// </summary>
    public interface IReticleProvider
    {
        public ReticleData ReticleData { get; }
        public float CurrentReticleSize { get; }
        public bool IsEnlarge { get; }

        /// <summary>
        /// レティクルを拡大する。
        /// </summary>
        public void OnEnlargeReticle();

        /// <summary>
        /// レティクルを縮小する。
        /// </summary>
        public void OnReduceReticle();
        
        /// <summary>
        /// レティクルを更新する。
        /// </summary>
        public void UpdateReticle();
    }
}