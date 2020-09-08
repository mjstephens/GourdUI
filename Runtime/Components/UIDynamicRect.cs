using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    public abstract class UIDynamicRect : MonoBehaviour, IUIDynamicRect
    {
        #region Properties

        public RectTransform dynamicTransform { get; protected set; }
        
        public IUIDynamicRectFilter filter { protected get; set; }
        
        public bool activeControl { get; protected set; }

        #endregion Properties

 
        #region Fields

        /// <summary>
        /// Collection of listeners that need to be informed when the dynaimc rect is updated.
        /// </summary>
        private readonly List<IUIDynamicRectListener> _listeners = new List<IUIDynamicRectListener>();

        #endregion Fields
 

        #region Listeners

        void IUIDynamicRect.SubscribeDynamicRectListener(IUIDynamicRectListener l)
        {
            if (!_listeners.Contains(l))
            {
                _listeners.Add(l);
            }
        }

        void IUIDynamicRect.UnsubscribeDynamicRectListener(IUIDynamicRectListener l)
        {
            _listeners.Remove(l);
        }

        #endregion Listeners


        #region Rect Updates

        void IUIDynamicRect.ForceUpdate()
        {
            OnUpdate();
        }

        protected void OnUpdate()
        {
            UpdateDynamicRect();
            foreach (IUIDynamicRectListener listener in _listeners)
            {
                listener.OnDynamicRectUpdate(this);
            }
        }

        protected abstract void UpdateDynamicRect();

        #endregion Rect Updates
    }
}