namespace Rigging.Demo.Shortcut
{
    public interface IShortcut
    {
        public bool HasItem { get; }
        public bool CanSelect { get; }
    }
}