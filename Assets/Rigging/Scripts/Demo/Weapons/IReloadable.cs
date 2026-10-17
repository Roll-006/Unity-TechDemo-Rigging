namespace Rigging.Demo.Weapons
{
    /// <summary>
    /// リロード可能なオブジェクト
    /// </summary>
    public interface IReloadable
    {
        /// <summary>
        /// リロードする。
        /// </summary>
        public void OnReload();
    }
}