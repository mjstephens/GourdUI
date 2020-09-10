using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GourdUI
{
    public class RectDragSource : UIDynamicRect
    {
        #region Variables

        [Header("References")] 
        [SerializeField]
        [Tooltip("The object to be dragged. Defaults to the local transform if left null.")]
        private RectTransform _dragObject;

        [Header("Values")]
        [SerializeField]
        [Tooltip("How quickly the drag object matches the position of the active input.")]
        private float _lerpSpeed = 25;
        [SerializeField]
        [Tooltip("How fast the drag object slows down after it is released. " +
                 "Use negative values to indicate no momentum.")]
        private float _flingSlowdown = 15;

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

        private const float CONST_draggerLimitedMaxDistance = 75;

        #endregion Variables
         

        #region Initialization

        private void Awake()
        {
            if (_dragObject == null)
            {
                _dragObject = transform.GetComponent<RectTransform>();
            }
            dynamicTransform = _dragObject;
            
            GameObject draggerObj = new GameObject();
            draggerObj.transform.name = "DragAnchor_" + _dragObject.name;
            draggerObj.transform.SetParent(dynamicTransform.parent);
            _dragger = draggerObj.AddComponent<RectTransform>();
            _dragger.position = dynamicTransform.position;
        }

        private void OnEnable()
        {
            _dragger.position = dynamicTransform.position;
        }

        #endregion Initialization


        #region Interaction

        public override void OnPointerDown(PointerEventData eventData)
        {           
            BeginDrag();
            base.OnPointerDown(eventData);
        }
        
        protected override void InteractionTick(Vector2 activeInputPosition)
        {
            OnDragFrame(activeInputPosition);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            EndDrag();
        }

        #endregion Interaction


        #region Drag

        /// <summary>
        /// Begins the dragging procedure for this dynamic rect
        /// </summary>
        private void BeginDrag()
        {
            _offset = GetActiveInputPosition() - (Vector2)dynamicTransform.position;
            _draggerPrevPos = _dragger.position;
            _releaseMomentum = Vector2.zero;
            _xAxisBoundaryReached = false;
            _yAxisBoundaryReached = false;

            if (_rectIsSlidingWithMomentum)
            {
                _rectIsSlidingWithMomentum = false;
                StopCoroutine(nameof(OnMomentumSlideFrame));
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
            OnInteractionEnd();
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