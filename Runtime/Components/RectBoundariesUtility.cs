using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    public static class RectBoundariesUtility
    {
        public static RectSpace TransformWithOffset(this RectSpace space, Vector2 transformation)
        {
            space.lowerLeft -= (Vector3)transformation;
            space.upperLeft -= (Vector3)transformation;
            space.upperRight -= (Vector3)transformation;
            space.lowerRight -= (Vector3)transformation;
            return space;
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
            List<RectSpace> splitSpaces = new List<RectSpace>();

            // Left space
            splitSpaces.Add(new RectSpace
            {
                lowerLeft = spaceToSplit.lowerLeft,
                upperLeft = spaceToSplit.upperLeft,
                upperRight = new Vector3(obstacle.left, spaceToSplit.top, 0),
                lowerRight = new Vector3(obstacle.left, spaceToSplit.bottom, 0),
            });
            
            // Top space
            splitSpaces.Add(new RectSpace
            {
                lowerLeft = new Vector3(spaceToSplit.left, obstacle.top, 0),
                upperLeft = spaceToSplit.upperLeft,
                upperRight = spaceToSplit.upperRight,
                lowerRight = new Vector3(spaceToSplit.right, obstacle.top, 0),
            });
            
            // Right space
            splitSpaces.Add(new RectSpace
            {
                lowerLeft = new Vector3(obstacle.right, spaceToSplit.bottom, 0),
                upperLeft = new Vector3(obstacle.right, spaceToSplit.top, 0),
                upperRight = spaceToSplit.upperRight,
                lowerRight = spaceToSplit.lowerLeft,
            });
            
            // Bottom space
            splitSpaces.Add(new RectSpace
            {
                lowerLeft = spaceToSplit.lowerLeft,
                upperLeft = new Vector3(spaceToSplit.left, obstacle.bottom, 0),
                upperRight = new Vector3(spaceToSplit.right, obstacle.bottom, 0),
                lowerRight = spaceToSplit.lowerRight,
            });

            // Remove any created spaces that are too small
            const float epsilon = 0.001f;
            for (int i = splitSpaces.Count - 1; i >= 0; i--)
            {
                if (Mathf.Abs(splitSpaces[i].top - splitSpaces[i].bottom) <= epsilon ||
                    Mathf.Abs(splitSpaces[i].left - splitSpaces[i].right) <= epsilon)
                {
                    splitSpaces.RemoveAt(i);
                }
            }

            return splitSpaces.ToArray();
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


        public static Vector2 GetRectSpaceOverlap(RectSpace element, RectSpace container)
        {
            Vector2 overlap = Vector2.zero;
            
            // X overlap
            if (element.left < container.left)
            {
                overlap = new Vector2(element.left - container.left, overlap.y);
            }

            if (element.right > container.right)
            {
                overlap = new Vector2(element.right - container.right, overlap.y);
            }
            
            // Y overlap
            if (element.top > container.top)
            {
                overlap = new Vector2(overlap.x, element.top - container.top);
            }

            if (element.bottom < container.bottom)
            {
                overlap = new Vector2(overlap.x, element.bottom - container.bottom);
            }

            return overlap;
        }
        
        
        private static bool RectContainsPoint(Vector3 point, RectSpace rect)
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
    }
}