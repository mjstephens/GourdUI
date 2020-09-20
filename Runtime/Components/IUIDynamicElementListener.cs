namespace GourdUI
{
    public interface IUIDynamicElementListener
    {
        void OnDynamicElementInteractionStart(IUIDynamicElement d);
        void OnDynamicElementInteractionEnd(IUIDynamicElement d);
        void OnDynamicElementUpdate(IUIDynamicElement d);
    }
}