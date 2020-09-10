using System;
using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    public class RectContainerGroup : MonoBehaviour
    {
        private readonly List<RectContainerElement> _elements = new List<RectContainerElement>();
        
        
        public void RegisterContainerElement(RectContainerElement element)
        {
            _elements.Add(element);
        }
        
        public void UnregisterContainerElement(RectContainerElement element)
        {
            _elements.Remove(element);
        }


        public void UpdateGroup()
        {
            
        }


        /// <summary>
        /// Returns the FreeSpaces within which the given element is completely contained, AFTER
        /// any overlap corrects have been applied.
        /// </summary>
        public RectSpace GetGroupedElementEvaluationBoundary(
            RectContainerElement activeElement, 
            RectSpace activeSpace, 
            RectSpace containerSpace,
            List<RectSpace> previousFreeSpaces,
            out List<RectSpace> newFreeSpaces)
        {
            // Get the free spaces
            RectSpace[] freespaces = CalculateFreeSpacesForGroupedElement(
                activeElement, 
                _elements,
                containerSpace);
            
            // Is our element fully contained within any space?
            List<RectSpace> fs = CalculateEnclosingFreeSpacesForGroupedElement(activeSpace, freespaces);

            // If we didn't find a space, that means our element is overlapping
            // We want to apply the correction temporarily, re-check to find the correct space,
            // And then try again to find the new evaluation container space
            if (fs.Count == 0)
            {
                // Get overlap distances for each previous freespace
                Vector2[] overlapDistances = new Vector2[previousFreeSpaces.Count];
                for (int i = 0; i < previousFreeSpaces.Count; i++)
                {
                    overlapDistances [i] = 
                        RectBoundariesUtility.GetRectSpaceOverlap(
                            activeSpace, previousFreeSpaces[i]);
                }
                
                // Which overlap distance was closest? That's the one we want!
                RectSpace evalSpace = previousFreeSpaces[0];
                Vector2 evalDistance = Vector2.zero;
                float currentClosestDistance = float.MaxValue;
                for (int i = 0; i < previousFreeSpaces.Count; i++)
                {
                    if (overlapDistances[i].sqrMagnitude < currentClosestDistance)
                    {
                        currentClosestDistance = overlapDistances[i].sqrMagnitude;
                        evalDistance = overlapDistances[i];
                        evalSpace = previousFreeSpaces[i];
                    }
                }

                // Now we can re-evaluate current spaces for transformed element
                RectSpace corrected = new RectSpace(activeSpace).TransformWithOffset(evalDistance);
                List<RectSpace> postCorrectionFS = 
                    CalculateEnclosingFreeSpacesForGroupedElement(corrected, freespaces);
                newFreeSpaces = postCorrectionFS.Count > 0 ? postCorrectionFS : new List<RectSpace> {containerSpace};
                
                //
                return evalSpace;
            }

            newFreeSpaces = fs.Count > 0 ? fs : new List<RectSpace> {containerSpace};
            return fs[0];
        }
        
        /// <summary>
        /// Calculates and returns the array of navigable RectSpaces formed in a group by all of the
        /// group elements, excluding the given active element.
        /// </summary>
        private static RectSpace[] CalculateFreeSpacesForGroupedElement(
            RectContainerElement activeElement,
            List<RectContainerElement> allGroupElements,
            RectSpace activeElementContainer)
        {
            // Gather list of obstacles
            List<RectSpace> obstacles = new List<RectSpace>();
            foreach (RectContainerElement e in allGroupElements)
            {
                if (e != activeElement)
                {
                    obstacles.Add(new RectSpace(e.source.dynamicTransform));
                }
            }
            
            List<RectSpace> freeSpaces = new List<RectSpace>();
            freeSpaces.Add(activeElementContainer);
            foreach (RectSpace obstacle in obstacles)
            {
                // Is this obstacle overlapping with any known freespaces?
                List<RectSpace> createdFreeSpaces = new List<RectSpace>();
                foreach (var space in freeSpaces)
                {
                    if (RectBoundariesUtility.GetRectSpacesDoOverlap(obstacle, space))
                    {
                        space.markedForRemoval = true;
                        createdFreeSpaces.AddRange(
                            RectBoundariesUtility.GetSplitSpacesFromObstacle(obstacle, space));
                    }
                }

                // Remove marked spaces
                for (int i = freeSpaces.Count - 1; i >= 0; i--)
                {
                    if (freeSpaces[i].markedForRemoval)
                    {
                        freeSpaces.Remove(freeSpaces[i]);
                    }
                }
                
                // Add created spaces
                freeSpaces.AddRange(createdFreeSpaces);
            }

            return freeSpaces.ToArray();
        }

        /// <summary>
        /// Calculates and returns a list of the freespaces within which the given element is
        /// completely contained (not overlapping any edges).
        /// </summary>
        private static List<RectSpace> CalculateEnclosingFreeSpacesForGroupedElement(
            RectSpace element, 
            RectSpace[] freespaces)
        {
            List<RectSpace> containingSpaces = new List<RectSpace>();
            foreach (RectSpace space in freespaces)
            {
                if (RectBoundariesUtility.GetRectSpaceOverlap(element, space) == Vector2.zero)
                {
                    containingSpaces.Add(space);
                }
            }

            return containingSpaces;
        }
    }
}