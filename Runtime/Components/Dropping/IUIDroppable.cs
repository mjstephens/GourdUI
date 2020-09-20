using UnityEngine;

namespace GourdUI
{
    public interface IUIDroppable
    {
        RectTransform droppableRect { get; }
        
        void OnDragElementContainerHoverEnter(DepositArea container);
        void OnDragElementContainerHoverExit(DepositArea container);
        void OnDragElementDrop(DepositArea container);
    }
}