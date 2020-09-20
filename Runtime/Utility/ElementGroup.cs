using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    /// <summary>
    /// Defines abstract grouping of elements - supports creation, retrieval
    /// </summary>
    public abstract class ElementGroup<G,E> where G : ElementGroup<G,E>, new()
    {
        #region Variables

        /// <summary>
        /// Static list of all of our groups
        /// </summary>
        private static readonly List<G> _groups = new List<G>();

        /// <summary>
        /// The key used to retrieve this group
        /// </summary>
        private int _groupKey;
        
        /// <summary>
        /// A list of all of the elements that belong to this group.
        /// </summary>
        protected readonly List<E> _elements = new List<E>();

        #endregion Variables
        

        #region Registration

        /// <summary>
        /// Adds the element to the group.
        /// </summary>
        public void RegisterContainerElement(E element)
        {
            _elements.Add(element);
        }
        
        /// <summary>
        /// Removes the element from the group.
        /// </summary>
        public void UnregisterContainerElement(E element)
        {
            _elements.Remove(element);
        }

        #endregion Registration
        
        
        #region Group

        public static G GetOrCreateGroup(Transform groupParent)
        {
            return groupParent != null ? GetOrCreateGroup(groupParent.GetHashCode()) : null;
        }
        
        private static G GetOrCreateGroup(int groupKey)
        {
            G toReturn = null;

            bool doesExist = false;
            foreach (G group in _groups)
            {
                if (group._groupKey == groupKey)
                {
                    doesExist = true;
                    toReturn = group;
                    break;
                }
            }

            // If we didn't find the group, go ahead and set its key
            if (!doesExist)
            {
                toReturn = new G();
                toReturn._groupKey = groupKey;
                _groups.Add(toReturn);
            }
            
            return toReturn;
        }

        #endregion Group
    }
}