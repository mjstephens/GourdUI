using System;
using UnityEngine;

namespace GourdUI
{
    public interface IUIDynamicRectPositionFilter : IUIDynamicRectFilter
    {
        Tuple<Vector2, bool, bool> GetFilteredPosition();
    }
}