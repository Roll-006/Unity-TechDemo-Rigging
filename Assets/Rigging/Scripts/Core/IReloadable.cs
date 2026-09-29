namespace Rigging.Core
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
