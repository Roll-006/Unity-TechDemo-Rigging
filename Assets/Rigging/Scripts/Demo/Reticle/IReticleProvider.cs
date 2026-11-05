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
        /// レティクルを拡大状態を設定する。
        /// </summary>
        public void SetEnlargeState(ReticleEnlargeState state);

        /// <summary>
        /// レティクルを拡大する。
        /// </summary>
        public void OnEnlargeReticle();
        
        /// <summary>
        /// レティクルを更新する。
        /// </summary>
        public void UpdateReticle();
    }
}