using UnityEngine;

namespace GourdUI
{
    [RequireComponent(typeof(DepositArea))]
    public abstract class DepositAreaDecorator : MonoBehaviour, IDepositAreaEventReceivable
    {
        #region Variables

        protected DepositArea _depositArea;

        #endregion Variables
            
            
        #region Initialization

        protected virtual void Awake()
        {
            _depositArea = GetComponent<DepositArea>();
        }

        protected virtual void OnEnable()
        { 
            _depositArea.SubscribeDecorator(this);
        }
            
        protected virtual void OnDisable()
        {
           _depositArea.UnsubscribeDecorator(this);
        }

        #endregion Initialization


        #region Deposit Events

        public virtual void OnDroppableRaycastReceiveEnter(IUIDroppable droppable)
        {
            
        }

        public virtual void OnDroppableRaycastReceiveStay(IUIDroppable droppable)
        {
            
        }

        public virtual void OnDroppableRaycastReceiveExit(IUIDroppable droppable)
        {
            
        }

        public virtual void OnDroppableDrop(IUIDroppable droppable)
        {
            
        }

        public virtual void OnDroppableRemove(IUIDroppable droppable)
        {
            
        }

        #endregion Deposit Events
    }
}

