using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class DepositArea : MonoBehaviour, IDepositAreaEventReceivable
    {
        #region Variables

        /// <summary>
        /// Contains the items (ordered) currently in this container
        /// </summary>
        public List<IUIDroppable> currentItems { get; protected set; }

        /// <summary>
        /// The transform of this deposit area object
        /// </summary>
        public RectTransform depositAreaTransform { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private readonly List<IDepositAreaEventReceivable> _decorators = new List<IDepositAreaEventReceivable>();
        
        #endregion Variables


        #region Initialization

        protected virtual void Awake()
        {
            currentItems = new List<IUIDroppable>();
            depositAreaTransform = GetComponent<RectTransform>();
        }

        #endregion Initialization
        
        
        #region Decorators

        public void SubscribeDecorator(IDepositAreaEventReceivable decorator)
        {
            if (!_decorators.Contains(decorator))
            {
                _decorators.Add(decorator);
            }
        }
        
        public void UnsubscribeDecorator(IDepositAreaEventReceivable decorator)
        { 
            _decorators.Remove(decorator);
        }

        #endregion Decorators
        
        
        #region Drop Events

        public virtual void OnDroppableRaycastReceiveEnter(IUIDroppable droppable)
        {
            // Inform decorators
            foreach (IDepositAreaEventReceivable d in _decorators)
            {
                d.OnDroppableRaycastReceiveEnter(droppable);
            }
        }

        public virtual void OnDroppableRaycastReceiveStay(IUIDroppable droppable)
        {
            // Inform decorators
            foreach (IDepositAreaEventReceivable d in _decorators)
            {
                d.OnDroppableRaycastReceiveStay(droppable);
            }
        }

        public virtual void OnDroppableRaycastReceiveExit(IUIDroppable droppable)
        {
            // Inform decorators
            foreach (IDepositAreaEventReceivable d in _decorators)
            {
                d.OnDroppableRaycastReceiveExit(droppable);
            }
        }

        public virtual void OnDroppableDrop(IUIDroppable droppable)
        {
            if (!currentItems.Contains(droppable))
            {
                currentItems.Add(droppable);
            }
            
            // Inform decorators
            foreach (IDepositAreaEventReceivable d in _decorators)
            {
                d.OnDroppableDrop(droppable);
            }
        }

        public virtual void OnDroppableRemove(IUIDroppable droppable)
        {
            currentItems.Remove(droppable);
            
            // Inform decorators
            foreach (IDepositAreaEventReceivable d in _decorators)
            {
                d.OnDroppableRemove(droppable);
            }
        }

        #endregion Drop Events
    }
}