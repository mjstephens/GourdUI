using UnityEngine;
using UnityEngine.Analytics;

namespace GourdUI
{
    public class RectSpace
    {
        public bool markedForRemoval;
        
        public float left => lowerLeft.x;
        public float top => upperLeft.y;
        public float right => upperRight.x;
        public float bottom => lowerRight.y;
        public Vector3 lowerLeft;
        public Vector3 upperLeft;
        public Vector3 upperRight;
        public Vector3 lowerRight;
    }
    
    
    public static class RectBoundariesUtility
    {
        public static RectSpace CreateRectSpace(RectTransform source)
        {
            Vector3[] corners = new Vector3[4];
            source.GetWorldCorners(corners);
            return CreateRectSpace(corners);
        }


        public static RectSpace CreateRectSpace(Vector3[] corners)
        {
            RectSpace r = new RectSpace
            {
                lowerLeft = corners[0],
                upperLeft = corners[1],
                upperRight = corners[2],
                lowerRight = corners[3]
            };
            return r;
        }


        public static bool GetRectSpacesDoOverlap(RectSpace space, RectSpace container)
        {
            return RectContainsPoint(space.lowerLeft, container) ||
                   RectContainsPoint(space.upperLeft, container) ||
                   RectContainsPoint(space.upperRight, container) ||
                   RectContainsPoint(space.lowerRight, container);
        }
        
        
        
        
        public static RectSpace[] GetSplitSpacesFromObstacle(RectSpace obstacle, RectSpace spaceToSplit)
        {
            // Left, top, right, bottom
            RectSpace[] splitSpaces = new RectSpace[4];

            // Left space
            splitSpaces[0] = new RectSpace
            {
                lowerLeft = spaceToSplit.lowerLeft,
                upperLeft = spaceToSplit.upperLeft,
                upperRight = new Vector3(obstacle.left, spaceToSplit.top, 0),
                lowerRight = new Vector3(obstacle.left, spaceToSplit.bottom, 0)
            };
            
            // Top space
            splitSpaces[1] = new RectSpace
            {
                lowerLeft = new Vector3(spaceToSplit.left, obstacle.top, 0),
                upperLeft = spaceToSplit.upperLeft,
                upperRight = spaceToSplit.upperRight,
                lowerRight = new Vector3(spaceToSplit.right, obstacle.top, 0)
            };
            
            // Right space
            splitSpaces[2] = new RectSpace
            {
                lowerLeft = new Vector3(obstacle.right, spaceToSplit.bottom, 0),
                upperLeft = new Vector3(obstacle.right, spaceToSplit.top, 0),
                upperRight = spaceToSplit.upperRight,
                lowerRight = spaceToSplit.lowerLeft
            };
            
            // Bottom space
            splitSpaces[3] = new RectSpace
            {
                lowerLeft = spaceToSplit.lowerLeft,
                upperLeft = new Vector3(spaceToSplit.left, obstacle.bottom, 0),
                upperRight = new Vector3(spaceToSplit.right, obstacle.bottom, 0),
                lowerRight = spaceToSplit.lowerRight
            };

            return splitSpaces;
        }
        
        
        
        public static Vector3[] GetScreenCorners()
        {
            float h = Screen.height;
            float w = Screen.width;
            
            return new []
            {
                new Vector3(0, 0, 0),
                new Vector3(0, h, 0), 
                new Vector3(w, h, 0),                 
                new Vector3(w, 0, 0),
            };
        }


        public static Vector2 GetRectSpaceOverlap(RectSpace space, RectSpace boundary)
        {
            Vector2 overlap = Vector2.zero;
            
           // Debug.Log(space.left + ", " + boundary.left);
            // X overlap
            if (space.left < boundary.left)
            {
                overlap = new Vector2(space.left - boundary.left, overlap.y);
            }
            else
            {
                if (space.right > boundary.right)
                {
                    overlap = new Vector2(space.right - boundary.right, overlap.y);
                }
            }
            
            // Y overlap
            if (space.top > boundary.top)
            {
                overlap = new Vector2(overlap.x, space.top - boundary.top);
            }
            else
            {
                if (space.bottom < boundary.bottom)
                {
                    overlap = new Vector2(overlap.x, space.bottom - boundary.bottom);
                }
            }

            return overlap;
        }
        
        
        public static bool RectContainsPoint(Vector3 point, RectSpace rect)
        {
            bool contains = true;
            if (point.x < rect.left || point.x > rect.right)
            {
                contains = false;
            }
            else if (point.y < rect.bottom || point.y > rect.top)
            {
                contains = false;
            }

            return contains;
        }

        public static bool SpaceContainsSpace(RectSpace spaceToCheck, RectSpace container)
        {
            return spaceToCheck.left > container.left &&
                   spaceToCheck.top < container.top &&
                   spaceToCheck.right < container.right &&
                   spaceToCheck.bottom > container.bottom;
        }
    }
}