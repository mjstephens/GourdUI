using System;
using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    [RequireComponent(typeof(RectTransform))]
    public class RectContainerElement : MonoBehaviour, IUIDynamicRectPositionFilter, IScreenRectUpdateListener
    {
        #region Enum

        /// <summary>
        /// EDGE = element will stop when any of its edges overlaps the container
        /// CENTER = element will be allowed to leave the contaner in any direction up to it center point
        /// </summary>
        public enum ElementBoundaryMode
        {
            Edge,
            Center
        }

        #endregion Enum
        

        #region Properties

        [Header("References")]
        public RectTransform container;

        [Header("Settings")] 
        public ElementBoundaryMode boundaryMode;
        
        #endregion Properties
        
        
        #region Fields
        
        /// <summary>
        /// The dynamic rect of the element being contained.
        /// </summary>
        public IUIDynamicRect source { get; private set; }

        /// <summary>
        /// The container rect itself may also be a dynamic rect.
        /// </summary>
        private IUIDynamicRect _containerRect;
 
        /// <summary>
        /// Cached bool, true if container is not screen boundaries
        /// </summary>
        private bool _hasExplicitContainer;

        private RectSpace _positionEvaluationSpace;

        /// <summary>
        /// List of free spaces within which this element is currently completely contained
        /// </summary>
        private List<RectSpace> _currentElementFreeSpaces = new List<RectSpace>();
        
        public RectContainerGroup group;

        private bool _hasInitialized;
        
        #endregion Fields


        #region Initialization

        private void Awake()
        {
            source = GetComponent<IUIDynamicRect>();
            if (source == null)
            {
                throw new NullReferenceException(
                    "RectContainerElement requires an IUIDynamicRect component!");
            }
            source.SubscribeDynamicRectListener(this);

            // Find container
            _hasExplicitContainer = container != null;
            if (_hasExplicitContainer && container.GetComponent<IUIDynamicRect>() != null)
            {
                _containerRect = container.GetComponent<IUIDynamicRect>();
                
                // Tell our containe to update this element whenever it moves
                _containerRect.SubscribeDynamicRectListener(this);
                _currentElementFreeSpaces.Add(new RectSpace(_containerRect.dynamicTransform));
            }
            else
            {
                _currentElementFreeSpaces.Add(new RectSpace(RectBoundariesUtility.GetScreenCorners()));
            }
            
            RefreshContainerBoundary();
            _hasInitialized = true;
        }

        private void OnEnable()
        {
            // Listen for changes in the device/screen
            GourdUI.Device.RegisterScreenUpdateListener(this);
            
            // We need callbacks when this element moves, and when the container moves
            _containerRect?.SubscribeDynamicRectListener(this);
            source?.SubscribeDynamicRectListener(this);
            
            // If this is part of a group, we need to subscribe
            if (group != null)
            {
                group.RegisterContainerElement(this);
            }
        }

        private void OnDisable()
        {
            GourdUI.Device.UnregisterScreenUpdateListener(this);
           _containerRect?.UnsubscribeDynamicRectListener(this);
            source?.UnsubscribeDynamicRectListener(this);
            
            // Group
            if (group != null)
            {
                group.UnregisterContainerElement(this);
            }
        }

        #endregion Initialization


        #region Interaction

        public void OnDynamicRectInteractionStart(IUIDynamicRect d)
        {
            OnContainerSpaceUpdated();
        }

        public void OnDynamicRectInteractionEnd(IUIDynamicRect d)
        {
            
        }

        #endregion Interaction
        

        #region Container
        
        void IUIDynamicRectListener.OnDynamicRectUpdate(IUIDynamicRect d)
        {
            if (d != source)
            {
                OnContainerSpaceUpdated();
            }
        }

        void IScreenRectUpdateListener.OnScreenRectUpdated(Rect rect)
        {
            RefreshContainerBoundary(true);
        }

        /// <summary>
        /// Refreshes the container space in which this object is contained
        /// </summary>
        private void RefreshContainerBoundary(bool updateElementPosition = false)
        {
            // If we're part of a group, we need to get the refined group container space
            if (!RefreshGroupFreeSpaces())
            {
                if (!_hasExplicitContainer)
                {
                    _positionEvaluationSpace = new RectSpace(RectBoundariesUtility.GetScreenCorners());
                }
                else
                {
                    _positionEvaluationSpace = new RectSpace(container);
                }
            }
            
            // Optionally update the dynamic rect
            if (updateElementPosition)
            {
                source?.ForceUpdate();
            }
        }

        private bool RefreshGroupFreeSpaces()
        {
            // If we're part of a group, we need to get the refined group container space
            if (group != null && _hasInitialized)
            {
                // Get container from group
                _positionEvaluationSpace = group.GetGroupedElementEvaluationBoundary(
                    this,
                    new RectSpace(source.dynamicTransform), 
                    new RectSpace(container),
                    _currentElementFreeSpaces,
                    out _currentElementFreeSpaces);

                return true;
            }
            return false;
        }

        #endregion Container


        #region Overlap

        /// <summary>
        /// Updates the bounds of the freespaces when this element's container is updated
        /// </summary>
        private void OnContainerSpaceUpdated()
        {
            // Update our source's space
            RefreshContainerBoundary(!source.activeControl);
        }

        Tuple<Vector2, bool, bool> IUIDynamicRectPositionFilter.GetFilteredPosition()
        {
            // Refersh container
            RefreshContainerBoundary();
            
            // Find the overlap with the current container
            Vector2 boundaryOverlap = RectBoundariesUtility.GetRectSpaceOverlap(
                new RectSpace(source.dynamicTransform),
                _positionEvaluationSpace,
                boundaryMode);

            return new Tuple<Vector2, bool, bool>(
                boundaryOverlap,
                boundaryOverlap.x != 0,
                boundaryOverlap.y != 0);
        }

        #endregion Overlap
    }
}