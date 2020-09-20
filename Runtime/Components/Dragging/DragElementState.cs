using UnityEngine;

namespace GourdUI
{
    public struct DragElementState
    {
        public bool activeControl;
        public Vector2 dtPosition;
        public Vector2 dtAnchoredPosition;
        public Vector2 offset;
        public Vector2 draggerPosition;
        public Vector2 draggerPreviousPosition;
        public Vector2 releaseMomentum;
        public bool xBoundaryReached;
        public bool yBoundaryReached;
        public bool sliding;
    }
}