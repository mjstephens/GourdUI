using System;
using UnityEngine;

namespace GourdUI
{
    public interface IUIDynamicElementPositionFilter : IUIDynamicElementFilter
    {
        Tuple<Vector2, bool, bool> GetFilteredPosition();
    }
}