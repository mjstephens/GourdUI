using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GourdUI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIDynamicRect : MonoBehaviour, IUIDynamicRect, IPointerDownHandler, IPointerUpHandler
    {
        #region Properties

        public RectTransform dynamicTransform { get; protected set; }
        
        public bool activeControl { get; protected set; }
        
        public Vector2 defaultPosition { get; private set; }

        #endregion Properties

 
        #region Fields

        /// <summary>
        /// Collection of listeners that need to be informed when the dynaimc rect is updated.
        /// </summary>
        private readonly List<IUIDynamicRectListener> _listeners = new List<IUIDynamicRectListener>();
        
        /// <summary>
        /// 
        /// </summary>
        private readonly List<IUIDynamicRectFilter> _filters = new List<IUIDynamicRectFilter>();

        /// <summary>
        /// True if we are currently interacting with this dynamic rect
        /// </summary>
        protected bool _interacting;

        protected bool _hasLoaded;

        protected bool _xAxisBoundaryReached;
        protected bool _yAxisBoundaryReached; 

        #endregion Fields


        #region Initialization

        protected virtual void Load()
        {
            defaultPosition = dynamicTransform.position;
            _hasLoaded = true;
        }
        
        protected virtual void OnDisable()
        {
            if (_interacting)
            {
                _interacting = false;
                StopCoroutine(nameof(DynamicRectInteractionTick));
                OnInteractionEnd();
            }
        }

        #endregion Initialization
 

        #region Listeners

        void IUIDynamicRect.SubscribeDynamicRectListener(IUIDynamicRectListener l)
        {
            // Listeners may get to us before we've loaded - make sure we're g2g
            Load();
            
            if (!_listeners.Contains(l))
            {
                _listeners.Add(l);
                if (l is IUIDynamicRectFilter f)
                {
                    _filters.Add(f);
                }
            }
        }

        void IUIDynamicRect.UnsubscribeDynamicRectListener(IUIDynamicRectListener l)
        {
            _listeners.Remove(l);
            if (l is IUIDynamicRectFilter f)
            {
                _filters.Remove(f);
            }
        }
        
        #endregion Listeners


        #region Interaction

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!_interacting)
            {
                _interacting = true;
                activeControl = true;
                StartCoroutine(nameof(DynamicRectInteractionTick));
                
                foreach (IUIDynamicRectListener listener in _listeners)
                {
                    listener.OnDynamicRectInteractionStart(this);
                }
            }
        }
        
        protected IEnumerator DynamicRectInteractionTick()
        {
            while (_interacting)
            {
                InteractionTick(GetActiveInputPosition());
                yield return null;
            }
        }
        
        protected abstract void InteractionTick(Vector2 activeInputPosition);

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (_interacting)
            {
                _interacting = false;
                StopCoroutine(nameof(DynamicRectInteractionTick));
            }
        }
        
        /// <summary>
        /// Called from child class when interaction has completely ended
        /// </summary>
        protected void OnInteractionEnd()
        {
            activeControl = false;
            foreach (IUIDynamicRectListener listener in _listeners)
            {
                listener.OnDynamicRectInteractionEnd(this);
            }
        }

        #endregion Interaction


        #region Input

        // TODO: Move to some dedicated input resolver thingy
        protected static Vector2 GetActiveInputPosition()
        {
            return Input.mousePosition;
        }

        #endregion Input


        #region Rect Updates

        void IUIDynamicRect.ForceUpdate()
        {
            UpdateDynamicRect(true);
        }

        /// <summary>
        /// Stages updates to be filtered and then applied to the dynamic rect.
        /// </summary>
        protected virtual void UpdateDynamicRect(bool forced = false)
        {
            // Apply filtered updates
            foreach (IUIDynamicRectFilter filter in _filters)
            {
                UIDynamicRectFilterEvaluator.EvaluateFilter(this, filter);
            }
            
            // Update listeners
            foreach (IUIDynamicRectListener listener in _listeners)
            {
                listener.OnDynamicRectUpdate(this);
            }
        }
        
        #endregion Rect Updates


        #region Filters

        public void ApplyPositionFilterResult(Tuple<Vector2, bool, bool> adjustmentData)
        {
            dynamicTransform.position -= (Vector3)adjustmentData.Item1;
            _xAxisBoundaryReached = adjustmentData.Item2;
            _yAxisBoundaryReached = adjustmentData.Item3;
        }

        #endregion Filters
    }
}