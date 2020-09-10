using System;
using UnityEngine;

namespace GourdUI
{
    public class RectSpace
    {
        #region Constructors

        public RectSpace() { }

        public RectSpace(RectSpace s)
        {
            markedForRemoval = false;
            lowerLeft = s.lowerLeft;
            upperLeft = s.upperLeft;
            upperRight = s.upperRight;
            lowerRight = s.lowerRight;
        }

        public RectSpace(Vector3[] corners)
        {
            markedForRemoval = false;
            lowerLeft = corners[0];
            upperLeft = corners[1];
            upperRight = corners[2];
            lowerRight = corners[3];
        }

        public RectSpace(RectTransform source)
        {
            markedForRemoval = false;
            Vector3[] corners = new Vector3[4];
            source.GetWorldCorners(corners);
            lowerLeft = corners[0];
            upperLeft = corners[1];
            upperRight = corners[2];
            lowerRight = corners[3];
        }

        #endregion Constructors
        
        
        public bool markedForRemoval;
        public float left => lowerLeft.x;
        public float top => upperLeft.y;
        public float right => upperRight.x;
        public float bottom => lowerRight.y;
        
        public float width => right - left;
        public float height => top - bottom;
        public Vector2 center => new Vector2(left + (width / 2), bottom + (height / 2));
        
        
        public Vector3 lowerLeft;
        public Vector3 upperLeft;
        public Vector3 upperRight;
        public Vector3 lowerRight;
    }
}