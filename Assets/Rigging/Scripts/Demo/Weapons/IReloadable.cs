namespace Rigging.Demo.Weapons
{
    /// <summary>
    /// リロード可能なオブジェクト
    /// </summary>
    public interface IReloadable
    {
        /// <summary>
        /// 弾薬数
        /// </summary>
        public int AmmoAmount { get; }
        
        /// <summary>
        /// 最大弾薬数
        /// </summary>
        public int MaxAmmoAmount { get; }
        
        /// <summary>
        /// リロードする。
        /// </summary>
        public void OnReload();
    }
}