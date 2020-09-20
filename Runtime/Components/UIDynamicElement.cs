using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIDynamicElement : MonoBehaviour, IUIDynamicElement 
    {
        #region Properties

        public RectTransform dynamicTransform { get; protected set; }
        
        public bool activeControl { get; protected set; }

        #endregion Properties

 
        #region Fields

        /// <summary>
        /// Collection of listeners that need to be informed when the dynaimc rect is updated.
        /// </summary>
        private readonly List<IUIDynamicElementListener> _listeners = new List<IUIDynamicElementListener>();
        
        /// <summary>
        /// 
        /// </summary>
        private readonly List<IUIDynamicElementFilter> _filters = new List<IUIDynamicElementFilter>();

        /// <summary>
        /// True if we are currently interacting with this dynamic rect
        /// </summary>
        protected bool _interacting;

        protected bool _xAxisBoundaryReached;
        protected bool _yAxisBoundaryReached; 

        #endregion Fields


        #region Initialization

        protected virtual void OnDisable()
        {
            if (_interacting)
            {
                _interacting = false;
                StopCoroutine(nameof(DynamicRectInteractionTick));
                EndElementInteraction();
            }
        }

        #endregion Initialization
 

        #region Listeners

        void IUIDynamicElement.SubscribeDynamicElementListener(IUIDynamicElementListener l)
        {
            if (!_listeners.Contains(l))
            {
                _listeners.Add(l);
                if (l is IUIDynamicElementFilter f)
                {
                    _filters.Add(f);
                }
            }
        }

        void IUIDynamicElement.UnsubscribeDynamicElementListener(IUIDynamicElementListener l)
        {
            _listeners.Remove(l);
            if (l is IUIDynamicElementFilter f)
            {
                _filters.Remove(f);
            }
        }
        
        #endregion Listeners


        #region Interaction

        protected void ActivateElementInteraction()
        {
            if (!_interacting)
            {
                _interacting = true;
                activeControl = true;
                StartCoroutine(nameof(DynamicRectInteractionTick));
                
                foreach (IUIDynamicElementListener listener in _listeners)
                {
                    listener.OnDynamicElementInteractionStart(this);
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

        /// <summary>
        /// Called from child class when interaction has completely ended
        /// </summary>
        protected void EndElementInteraction()
        {
            if (_interacting)
            {
                _interacting = false;
                StopCoroutine(nameof(DynamicRectInteractionTick));
            }
            
            activeControl = false;
            foreach (IUIDynamicElementListener listener in _listeners)
            {
                listener.OnDynamicElementInteractionEnd(this);
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

        void IUIDynamicElement.ForceUpdate()
        {
            UpdateDynamicRect(true);
        }

        /// <summary>
        /// Stages updates to be filtered and then applied to the dynamic rect.
        /// </summary>
        protected virtual void UpdateDynamicRect(bool forced = false)
        {
            // Apply filtered updates
            foreach (IUIDynamicElementFilter filter in _filters)
            {
                UIDynamicElementFilterEvaluator.EvaluateFilter(this, filter);
            }
            
            // Update listeners
            foreach (IUIDynamicElementListener listener in _listeners)
            {
                listener.OnDynamicElementUpdate(this);
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