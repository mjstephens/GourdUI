using UnityEngine;

namespace GourdUI
{
    [RequireComponent(typeof(DragElement))]
    public abstract class DragElementDecorator : MonoBehaviour, IDropElementEventReceivable
    {
        #region Variables

        protected DragElement _dragElement;

        #endregion Variables
        
        
        #region Initialization

        protected virtual void Awake()
        {
            _dragElement = GetComponent<DragElement>();
        }

        protected virtual void OnEnable()
        {
            _dragElement.SubscribeDecorator(this);
        }
        
        protected virtual void OnDisable()
        {
            _dragElement.UnsubscribeDecorator(this);
        }

        #endregion Initialization


        #region Droppable Events

        public virtual void OnDragElementContainerHoverEnter(DepositArea container)
        {
            
        }

        public virtual void OnDragElementContainerHoverExit(DepositArea outContainer, DepositArea inContainer)
        {
            
        }


        public virtual void OnDragElementDrop(DepositArea container)
        {
            
        }

        #endregion Droppable Events
    }
}