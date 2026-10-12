namespace Rigging.Core
{
    public interface IShortcut
    {
        public bool HasItem { get; }
        public bool CanSelect { get; }
    }
}