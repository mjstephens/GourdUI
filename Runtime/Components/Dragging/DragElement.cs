using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GourdUI
{
    [RequireComponent(typeof(Image))]
    public partial class DragElement : UIDynamicElement, IUIDroppable, IPointerDownHandler, IPointerUpHandler
    {
        #region Static

        public static readonly List<IUIDroppable> activeDragElements = new List<IUIDroppable>();

        #endregion Static
        
        
        #region Variables

        [Header("Values")]
        [SerializeField]
        [Tooltip("How quickly the drag object matches the position of the active input.")]
        private float _lerpSpeed = 25;
        
        [SerializeField]
        [Tooltip("How fast the drag object slows down after it is released. " +
                 "Use negative values to indicate no momentum.")]
        private float _flingSlowdown = 15;
        
        [Header("Focus")] 
        [SerializeField]
        protected bool _setAsLastSiblingOnFocus;

        [Header("Containment")] 
        [SerializeField]
        [Tooltip("")]
        private RectContainerElement.DragElementContainerType _containerType;
        
        [SerializeField]
        [Tooltip("")]
        private RectContainerElement.DragElementContainerBoundary _containerBoundary;

        [SerializeField] 
        private Transform _groupParent;

        [Header("Dropping")] 
        [SerializeField]
        private bool _checkForDropAreasOnDrag = true;
        
        public RectTransform droppableRect { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private RectTransform _dragObject;

        /// <summary>
        /// Describes the offset of the pointer when the drag is initiated.
        /// </summary>
        private Vector2 _offset;

        /// <summary>
        /// 
        /// </summary>
        private Vector2 _offsetPosition => (Vector2)dynamicTransform.position + _offset;
        
        /// <summary>
        /// The dragger obect we use to perform movement calculations.
        /// </summary>
        private RectTransform _dragger;

        /// <summary>
        /// We cache the dragger position to calculate momentum when drag is released
        /// </summary>
        private Vector2 _draggerPrevPos;
        
        /// <summary>
        /// Calculated based on value of @_draggerPrevPos when the drag is released.
        /// </summary>
        private Vector2 _releaseMomentum;
        
        /// <summary>
        /// Flag for running @OnMomentumSlideFrame enumerator.
        /// </summary>
        private bool _rectIsSlidingWithMomentum;

        /// <summary>
        /// 
        /// </summary>
        private RectContainerElement _container;

        private Transform _defaultParent;

        private const float CONST_draggerLimitedMaxDistance = 75;

        #endregion Variables
         

        #region Initialization

        private void Awake()
        {
            // Attach the drag object
            // TODO: Allow drag object to be different than source object
            if (_dragObject == null)
            {
                _dragObject = transform.GetComponent<RectTransform>();
            }
            dynamicTransform = _dragObject;
            droppableRect = dynamicTransform;
            _defaultParent = dynamicTransform.parent;
            
            // Create the dragger transform that we will use to calculate drag values
            GameObject draggerObj = new GameObject();
            draggerObj.transform.name = "DragAnchor_" + _dragObject.name;
            draggerObj.transform.SetParent(dynamicTransform.parent);
            _dragger = draggerObj.AddComponent<RectTransform>();
            _dragger.position = dynamicTransform.position;
            
            // If we have a container, give it a class to process containment values
            if (_containerType != RectContainerElement.DragElementContainerType.None)
            {
                // Create our new container
                _container = new RectContainerElement(this, _groupParent, _containerBoundary);
                
                // Find out explicit container, plus the dynamic component if applicable
                RectTransform explicitContainer = null;
                IUIDynamicElement dynamicContainer = null;
                if (_containerType == RectContainerElement.DragElementContainerType.Parent)
                {
                    explicitContainer = transform.parent.GetComponent<RectTransform>();

                    if (explicitContainer != null)
                    {
                        dynamicContainer = explicitContainer.GetComponent<IUIDynamicElement>();
                    }
                }
                
                //
                _container.SetContainerType(_containerType, explicitContainer, dynamicContainer);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _container?.Disable();
        }

        private void OnDestroy()
        {
            if (_dragger != null)
            {
                Destroy(_dragger.gameObject);
            }
        }

        #endregion Initialization


        #region Interaction

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            BeginDrag();
            activeDragElements.Add(this);
            ActivateElementInteraction();
        }
        
        protected override void InteractionTick(Vector2 activeInputPosition)
        {
            OnDragFrame(activeInputPosition);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            EndElementInteraction();
            activeDragElements.Remove(this);
            EndDrag();
        }

        #endregion Interaction


        #region Drag

        /// <summary>
        /// Begins the dragging procedure for this dynamic rect
        /// </summary>
        private void BeginDrag()
        {
            // Reset drag values
            _offset = GetActiveInputPosition() - (Vector2)dynamicTransform.position;
            _draggerPrevPos = _dragger.position;
            _releaseMomentum = Vector2.zero;
            _xAxisBoundaryReached = false;
            _yAxisBoundaryReached = false;
            GetComponent<Image>().raycastTarget = false;

            // Bring element to front
            if (_setAsLastSiblingOnFocus)
            {
                dynamicTransform.SetAsLastSibling();
            }

            // Cancel any existing momentum for this draggable
            if (_rectIsSlidingWithMomentum)
            {
                _rectIsSlidingWithMomentum = false;
                StopCoroutine(nameof(OnMomentumSlideFrame));
            }

            // Listen for our draggable entering drop areas 
            if (_checkForDropAreasOnDrag)
            {
                StartCoroutine(nameof(DropcastHover));
            }
        }

        /// <summary>
        /// Called every frame this rect is being actively dragged
        /// </summary>
        private void OnDragFrame(Vector2 activeInputPosition)
        {
            _draggerPrevPos = _dragger.position;
            _dragger.position = activeInputPosition;

            LimitDraggerDistance();

            UpdateDynamicRect();
        }
        
        /// <summary>
        /// Called when the drag control on this dynamic rect is released
        /// </summary>
        private void EndDrag()
        {
            _releaseMomentum = (Vector2)_dragger.position - _draggerPrevPos;

            if (_releaseMomentum != Vector2.zero)
            {
                _rectIsSlidingWithMomentum = true;
                StartCoroutine(nameof(OnMomentumSlideFrame));
            }
            
            // Listen for our draggable entering drop areas 
            if (_checkForDropAreasOnDrag)
            {
                StopCoroutine(nameof(DropcastHover));
                Dropcast();
            }
            GetComponent<Image>().raycastTarget = true;
        }

        private void LimitDraggerDistance()
        {
            // Limit dragger distance if we are at a boundary... this prevents extreme dragger distances from 
            // "skipping" free spaces, resulting in grouped elements warping through each other
            if (Vector3.Distance(_dragger.position, _offsetPosition) > CONST_draggerLimitedMaxDistance &&
                (_xAxisBoundaryReached || _yAxisBoundaryReached))
            {
                Vector3 direction = (_dragger.position - (Vector3)_offsetPosition).normalized;
                _dragger.position = (Vector3)_offsetPosition + (direction * CONST_draggerLimitedMaxDistance);
            }
        }

        #endregion Drag


        #region Momentum

        /// <summary>
        /// Moves the rect with momentum after the drag has been released.
        /// </summary>
        private IEnumerator OnMomentumSlideFrame()
        {
            while (_rectIsSlidingWithMomentum)
            {
                // Apply dragger post-release momentum
                _releaseMomentum = Vector2.Lerp(
                    _releaseMomentum,
                    Vector2.zero,
                    Time.unscaledDeltaTime * _flingSlowdown);
                _dragger.position = (Vector2)_dragger.position + _releaseMomentum;
                
                LimitDraggerDistance();

                // If we've reached the edge of a boundary, we don't want to apply momentum in that direction
                if (_xAxisBoundaryReached)
                {
                    _releaseMomentum.x = 0;
                }

                if (_yAxisBoundaryReached)
                {
                    _releaseMomentum.y = 0;
                }
                
                float xDistance = _xAxisBoundaryReached
                    ? 0
                    : Mathf.Abs(_offsetPosition.x - _dragger.position.x);
                float yDistance = _yAxisBoundaryReached
                    ? 0
                    : Mathf.Abs(_offsetPosition.y - _dragger.position.y);
                    
                if (xDistance + yDistance <= 0.01f)
                {
                    _rectIsSlidingWithMomentum = false;
                }
                
                UpdateDynamicRect();
                yield return null;
            }
            
            // Once the sliding has stopped, we know the interaction is completely over
            EndElementInteraction();
        }

        #endregion Momentum


        #region Update

        protected override void UpdateDynamicRect(bool forced = false)
        {
            if (!forced)
            {
                dynamicTransform.position = Vector2.Lerp(
                    dynamicTransform.position,
                    (Vector2) _dragger.position - _offset,
                    Time.unscaledDeltaTime * _lerpSpeed);
            }
            
            base.UpdateDynamicRect(forced);
        }

        #endregion Update
    }
}