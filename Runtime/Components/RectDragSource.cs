using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GourdUI
{
    [RequireComponent(typeof(RectTransform))]
    public class RectDragSource : UIDynamicRect, IPointerDownHandler, IPointerUpHandler
    {
        #region Variables

        [Header("References")] 
        public RectTransform dragTarget;

        [Header("Values")]
        public float lerpSpeed = 25;
        public float flingSlowdown = 15;

        private Vector2 _offset;
        private bool _ticking;
        private bool _dragging;
        private RectTransform _dragger;
        
        private Vector2 _draggerPrevPos;
        private Vector2 _releaseMomentum;

        private bool _xAxisBoundaryReached;
        private bool _yAxisBoundaryReached; 

        #endregion Variables
         

        #region Initialization

        private void Awake()
        {
            if (dragTarget == null)
            {
                dragTarget = transform.GetComponent<RectTransform>();
            }
            dynamicTransform = dragTarget;
            
            GameObject draggerObj = new GameObject();
            draggerObj.transform.name = "DragAnchor_" + dragTarget.name;
            draggerObj.transform.SetParent(dynamicTransform.parent);
            _dragger = draggerObj.AddComponent<RectTransform>();
            _dragger.position = dynamicTransform.position;
        }

        private void OnEnable()
        {
            _dragger.position = dynamicTransform.position;
        }

        #endregion Initialization


        #region Pointer

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            _offset = Input.mousePosition - dynamicTransform.position;
            _dragging = true;
            _draggerPrevPos = _dragger.position;
            _releaseMomentum = Vector2.zero;
            _xAxisBoundaryReached = false;
            _yAxisBoundaryReached = false;
            activeControl = true;

            if (!_ticking)
            {
                _ticking = true;
                StartCoroutine(nameof(DragTick));
            }
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            _dragging = false;
            _releaseMomentum = (Vector2)_dragger.position - _draggerPrevPos;
        }

        #endregion Pointer


        #region Drag

        private IEnumerator DragTick()
        {
            while (_ticking)
            {
                if (_dragging)
                {
                    _draggerPrevPos = _dragger.position;
                    Vector2 inputPosition = Input.mousePosition;
                    _dragger.position = inputPosition;
                }
                else
                {
                    // Apply dragger post-release momentum
                    _releaseMomentum = Vector2.Lerp(
                        _releaseMomentum,
                        Vector2.zero,
                        Time.unscaledDeltaTime * flingSlowdown);
                    _dragger.position = (Vector2)_dragger.position + _releaseMomentum;

                    float xDistance = _xAxisBoundaryReached
                        ? 0
                        : Mathf.Abs(((Vector2) dynamicTransform.position + _offset).x - _dragger.position.x);
                    float yDistance = _yAxisBoundaryReached
                        ? 0
                        : Mathf.Abs(((Vector2) dynamicTransform.position + _offset).y - _dragger.position.y);
                    
                    if (xDistance + yDistance <= 0.01f)
                    {
                        _ticking = false;
                        activeControl = false;
                        dynamicTransform.position = (Vector2)_dragger.position - _offset;
                    }
                }

                OnUpdate();

                yield return null;
            }
        }

        protected override void UpdateDynamicRect()
        {
            // Set position of rect
            if (activeControl)
            {
                dynamicTransform.position = Vector2.Lerp(
                    dynamicTransform.position,
                    (Vector2)_dragger.position - _offset,
                    Time.unscaledDeltaTime * lerpSpeed);
            }
            
            // Look for filter adjustments
            if (filter != null)
            {
                Vector2 adjustment = filter.FilterPositionAdjustment();
                dynamicTransform.position = (Vector2) dynamicTransform.position - adjustment;

                _xAxisBoundaryReached = adjustment.x != 0;
                _yAxisBoundaryReached = adjustment.y != 0;
            }
        }

        #endregion Drag
    }
}