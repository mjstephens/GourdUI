using System;
using UnityEngine;

namespace GourdUI
{
    [RequireComponent(typeof(RectTransform))]
    public class RectContainerElement : MonoBehaviour, IUIDynamicRectFilter, IScreenRectUpdateListener
    {
        #region Properties

        [Header("References")]
        public RectTransform container;
        
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

        /// <summary>
        /// The world-space corners of our object
        /// </summary>
        public RectSpace sourceSpace { get; private set; }
        
        /// <summary>
        /// The world-space corners of our container
        /// </summary>
        private RectSpace _currentContainerSpace;
        
        public RectContainerGroup group;

        private bool _hasInitialized;
        
        #endregion Fields


        #region Initialization

        private void Start()
        {
            // Find element
            source = GetComponent<IUIDynamicRect>();
            if (source == null)
            {
                throw new NullReferenceException(
                    "RectContainerElement requires an IUIDynamicRect component!");
            }
            source.filter = this;

            // Find container
            _hasExplicitContainer = container != null;
            if (_hasExplicitContainer && container.GetComponent<IUIDynamicRect>() != null)
            {
                _containerRect = container.GetComponent<IUIDynamicRect>();
                _containerRect.SubscribeDynamicRectListener(this);
            }
            
            RefreshContainerBoundary();
            _hasInitialized = true;
        }

        private void OnEnable()
        {
            GourdUI.Device.RegisterScreenUpdateListener(this);
            _containerRect?.SubscribeDynamicRectListener(this);
            
            // Group
            if (group != null)
            {
                group.RegisterContainerElement(this);
            }
        }

        private void OnDisable()
        {
            GourdUI.Device.UnregisterScreenUpdateListener(this);
            _containerRect?.UnsubscribeDynamicRectListener(this);
            
            // Group
            if (group != null)
            {
                group.UnregisterContainerElement(this);
            }
        }

        #endregion Initialization
        

        #region Container
        
        void IUIDynamicRectListener.OnDynamicRectUpdate(IUIDynamicRect d)
        {
            RefreshContainerBoundary();
        }

        void IScreenRectUpdateListener.OnScreenRectUpdated(Rect rect)
        {
            RefreshContainerBoundary();
        }

        /// <summary>
        /// Refreshes the container space in which this object is contained
        /// </summary>
        private void RefreshContainerBoundary(bool forceUpdate = true)
        {
            // If we're part of a group, we need to get the refined group container space
            if (group != null && source.activeControl && _hasInitialized)
            {
                // Get container from group
                _currentContainerSpace = group.GetGroupEvaluationContainer(
                    this, 
                    _currentContainerSpace,
                    container);
            }
            else
            {
                if (!_hasExplicitContainer)
                {
                    _currentContainerSpace = RectBoundariesUtility.CreateRectSpace(
                        RectBoundariesUtility.GetScreenCorners());
                }
                else
                {
                    _currentContainerSpace = RectBoundariesUtility.CreateRectSpace(container);
                }
            }
            
            // Optionally update the dynamic rect
            if (forceUpdate)
            {
                source.ForceUpdate();
            }
        }

        #endregion Container


        #region Overlap

        Vector2 IUIDynamicRectFilter.FilterPositionAdjustment()
        {
            // Update our source's space
            sourceSpace = RectBoundariesUtility.CreateRectSpace(source.dynamicTransform);
            if (source.activeControl)
            {
                RefreshContainerBoundary(false);
            }
            
            // Find any overlap outside of our object's container
            Vector2 boundaryOverlap = 
                RectBoundariesUtility.GetRectSpaceOverlap(sourceSpace, _currentContainerSpace);

            return boundaryOverlap;
        }

        #endregion Overlap

        
    }
}