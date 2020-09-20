using System;
using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    public class RectContainerElement : IUIDynamicElementPositionFilter, IScreenRectUpdateListener
    {
        #region Enum

        public enum DragElementContainerType
        {
            None,
            Screen,
            SafeArea,
            Parent
        }
        
        /// <summary>
        /// EDGE = element will stop when any of its edges overlaps the container
        /// CENTER = element will be allowed to leave the contaner in any direction up to it center point
        /// </summary>
        public enum DragElementContainerBoundary
        {
            Edge,
            Center
        }

        #endregion Enum
        

        #region Properties

        private readonly DragElementContainerBoundary _boundaryMode;
        
        #endregion Properties
        
        
        #region Fields

        /// <summary>
        /// The dynamic rect of the element being contained.
        /// </summary>
        public IUIDynamicElement _source { get; }

        /// <summary>
        /// The container rect itself may also be a dynamic rect.
        /// </summary>
        private RectSpace _containerSpace;
        
        /// <summary>
        /// 
        /// </summary>
        private DragElementContainerType _containerType;

        /// <summary>
        /// The space in which this element will be evaluated for overlap.
        /// </summary>
        private RectSpace _positionEvaluationSpace;

        /// <summary>
        /// List of free spaces within which this element is currently completely contained
        /// </summary>
        private List<RectSpace> _currentElementFreeSpaces = new List<RectSpace>();
        
        /// <summary>
        /// The group (if applicable) to which this element belongs
        /// </summary>
        private readonly RectContainerGroup _group;

        private RectTransform _explicitContainer;
        private IUIDynamicElement _dynamicContainer;
        
        #endregion Fields


        #region Initialization

        public RectContainerElement(
            IUIDynamicElement inSource,
            Transform groupParent,
            DragElementContainerBoundary boundaryMode)
        {
            _source = inSource;
            _boundaryMode = boundaryMode;
            
            _source.SubscribeDynamicElementListener(this);
            _group = RectContainerGroup.GetOrCreateGroup(groupParent);
            _group?.RegisterContainerElement(this);
        }

        public void SetContainerType(
            DragElementContainerType containerType, 
            RectTransform container,
            IUIDynamicElement dynamicContainer)
        {
            _explicitContainer = container;
            _dynamicContainer = dynamicContainer;
            _containerType = containerType;
            switch (_containerType)
            {
                case DragElementContainerType.Screen:
                case DragElementContainerType.SafeArea:
                    _containerSpace = new RectSpace(RectBoundariesUtility.GetScreenCorners());
                    GourdUI.Device.RegisterScreenUpdateListener(this);
                    break;
                
                case DragElementContainerType.Parent:
                    _containerSpace = new RectSpace(_explicitContainer);
                    _dynamicContainer?.SubscribeDynamicElementListener(this);
                    break;
            }
            _currentElementFreeSpaces.Add(_containerSpace);
        }

        public void Disable()
        {
            // Unsub from source
            _source.UnsubscribeDynamicElementListener(this);
            _group?.UnregisterContainerElement(this);

            // Unsub from any other 
            switch (_containerType)
            {
                case DragElementContainerType.Screen:
                case DragElementContainerType.SafeArea:
                    GourdUI.Device.UnregisterScreenUpdateListener(this);
                    break;
                case DragElementContainerType.Parent:
                    _dynamicContainer?.UnsubscribeDynamicElementListener(this);
                    break;
            }
        }

        #endregion Initialization


        #region Interaction

        void IUIDynamicElementListener.OnDynamicElementInteractionStart(IUIDynamicElement d)
        {
            
        }

        void IUIDynamicElementListener.OnDynamicElementInteractionEnd(IUIDynamicElement d)
        {
            
        }

        #endregion Interaction
        

        #region Container
        
        void IUIDynamicElementListener.OnDynamicElementUpdate(IUIDynamicElement d)
        {
            // If this update is not from the source, that means it's coming from our container
            if (d != _source)
            {
                _containerSpace = new RectSpace(_explicitContainer);
                _source.ForceUpdate();
            }
        }

        void IScreenRectUpdateListener.OnScreenRectUpdated(Rect rect)
        {
            _containerSpace = new RectSpace(RectBoundariesUtility.GetScreenCorners());
            _source.ForceUpdate();
        }

        private void RefreshGroupFreeSpaces()
        {
            // Get container from group
            _positionEvaluationSpace = _group.GetGroupedElementEvaluationBoundary(
                this,
                _boundaryMode,
                new RectSpace(_source.dynamicTransform),
                _containerSpace,
                _currentElementFreeSpaces,
                out _currentElementFreeSpaces);
        }

        #endregion Container


        #region Overlap

        Tuple<Vector2, bool, bool> IUIDynamicElementPositionFilter.GetFilteredPosition()
        {
            // Refersh container
            if (_group != null)
            {
                RefreshGroupFreeSpaces();
            }
            else
            {
                _positionEvaluationSpace = _containerSpace;
            }
            
            // Find the overlap with the current container
            Vector2 boundaryOverlap = RectBoundariesUtility.GetRectSpaceOverlap(
                new RectSpace(_source.dynamicTransform),
                _positionEvaluationSpace,
                _boundaryMode);

            return new Tuple<Vector2, bool, bool>(
                boundaryOverlap,
                boundaryOverlap.x != 0,
                boundaryOverlap.y != 0);
        }

        #endregion Overlap
    }
}