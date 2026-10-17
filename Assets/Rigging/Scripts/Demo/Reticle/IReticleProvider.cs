namespace Rigging.Demo.Reticle
{
    /// <summary>
    /// レティクルを持つオブジェクト
    /// </summary>
    public interface IReticleProvider
    {
        public ReticleModel ReticleModel { get; }
        public ReticleEnlargeState ReticleEnlargeState { get; }

        /// <summary>
        /// レティクルを拡大する。
        /// </summary>
        public void OnEnlargeReticle();

        /// <summary>
        /// レティクルを縮小する。
        /// </summary>
        public void OnReduceReticle();
        
        /// <summary>
        /// レティクルを停止する。
        /// </summary>
        public void OnStopReticle();
        
        /// <summary>
        /// レティクルを更新する。
        /// </summary>
        public void UpdateReticle();
    }
}