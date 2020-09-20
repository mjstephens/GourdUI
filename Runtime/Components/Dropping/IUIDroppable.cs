using UnityEngine;

namespace GourdUI
{
    public interface IDropElementEventReceivable
    {
        void OnDragElementContainerHoverEnter(DepositArea container);
        void OnDragElementContainerHoverExit(DepositArea outContainer, DepositArea inContainer);
        void OnDragElementDrop(DepositArea container);
    }

    public interface IDepositAreaEventReceivable
    {
        void OnDroppableRaycastReceiveEnter(IUIDroppable droppable);
        void OnDroppableRaycastReceiveStay(IUIDroppable droppable);
        void OnDroppableRaycastReceiveExit(IUIDroppable droppable);
        void OnDroppableDrop(IUIDroppable droppable);
        void OnDroppableRemove(IUIDroppable droppable);
    }
    
    public interface IUIDroppable : IDropElementEventReceivable
    {
        RectTransform droppableRect { get; }
    }
}