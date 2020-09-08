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

        
            
            
        public RectSpace GetGroupEvaluationContainer(
            RectContainerElement activeElement, 
            RectSpace currentContainerSpace,
            RectTransform fallbackContainer)
        {
            // Get the free spaces
            RectSpace[] freespaces = GetFreeSpaces(activeElement);
            
            // Is our element fully contained within any space?
            RectSpace containingSpace = null;
            foreach (var space in freespaces)
            {
                if (RectBoundariesUtility.SpaceContainsSpace(activeElement.sourceSpace, space))
                {
                    // If we find multiple spaces, fallback to container space
                    if (containingSpace != null)
                    {
                        containingSpace = RectBoundariesUtility.CreateRectSpace(fallbackContainer);
                        break;
                    }
                    
                    containingSpace = space;
                }
            }
            
            // If containing space is null, we've collided with something. Return what we had before
            return containingSpace ?? 
                   (currentContainerSpace ?? RectBoundariesUtility.CreateRectSpace(fallbackContainer));
        }

        
        
        private RectSpace[] GetFreeSpaces(RectContainerElement activeElement)
        {
            // Gather list of obstacles
            List<RectSpace> obstacles = new List<RectSpace>();
            foreach (var e in _elements)
            {
                if (e != activeElement)
                {
                    obstacles.Add(e.sourceSpace);
                }
            }
            
            List<RectSpace> freeSpaces = new List<RectSpace>();
            freeSpaces.Add(RectBoundariesUtility.CreateRectSpace(activeElement.container));
            foreach (var obstacle in obstacles)
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


        
    }
}