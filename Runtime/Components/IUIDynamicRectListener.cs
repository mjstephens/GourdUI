namespace GourdUI
{
    public interface IUIDynamicRectListener
    {
        IUIDynamicRect source { get; }
        
        void OnDynamicRectUpdate(IUIDynamicRect d);
    }
}