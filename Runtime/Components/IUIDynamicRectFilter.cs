using UnityEngine;

namespace GourdUI
{
    public interface IUIDynamicRectFilter : IUIDynamicRectListener
    {
        Vector2 FilterPositionAdjustment();
    }
}