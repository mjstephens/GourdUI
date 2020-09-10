namespace GourdUI
{
    public interface IUIDynamicRectListener
    {
        IUIDynamicRect source { get; }

        void OnDynamicRectInteractionStart(IUIDynamicRect d);
        void OnDynamicRectInteractionEnd(IUIDynamicRect d);
        void OnDynamicRectUpdate(IUIDynamicRect d);
    }
}