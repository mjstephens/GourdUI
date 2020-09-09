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
        public Vector3 lowerLeft;
        public Vector3 upperLeft;
        public Vector3 upperRight;
        public Vector3 lowerRight;

        
        
        #region Equality

        // public bool Equals(RectSpace other)
        // {
        //     if (ReferenceEquals(null, other)) return false;
        //     if (ReferenceEquals(this, other)) return true;
        //     return markedForRemoval == other.markedForRemoval && lowerLeft.Equals(other.lowerLeft) && upperLeft.Equals(other.upperLeft) && upperRight.Equals(other.upperRight) && lowerRight.Equals(other.lowerRight);
        // }
        //
        // public override bool Equals(object obj)
        // {
        //     if (ReferenceEquals(null, obj)) return false;
        //     if (ReferenceEquals(this, obj)) return true;
        //     if (obj.GetType() != this.GetType()) return false;
        //     return Equals((RectSpace) obj);
        // }
        //
        // public override int GetHashCode()
        // {
        //     unchecked
        //     {
        //         var hashCode = markedForRemoval.GetHashCode();
        //         hashCode = (hashCode * 397) ^ lowerLeft.GetHashCode();
        //         hashCode = (hashCode * 397) ^ upperLeft.GetHashCode();
        //         hashCode = (hashCode * 397) ^ upperRight.GetHashCode();
        //         hashCode = (hashCode * 397) ^ lowerRight.GetHashCode();
        //         return hashCode;
        //     }
        // }

        #endregion Equality
    }
}