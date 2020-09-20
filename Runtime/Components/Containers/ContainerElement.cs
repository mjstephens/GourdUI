using UnityEngine;

namespace GourdUI
{
    public class ContainerElement : DepositArea
    {
        #region Variables

        [Header("Settings")]
        [SerializeField]
        private bool _snapDroppablesToAreaOnDrop = true;

        #endregion Variables
        
        
        #region Drop Events

        public override void OnDroppableDrop(IUIDroppable droppable)
        {
            base.OnDroppableDrop(droppable);
            
            if (_snapDroppablesToAreaOnDrop)
            {
                Vector2 boundaryOverlap = RectBoundariesUtility.GetRectSpaceOverlap(
                    new RectSpace(droppable.droppableRect),
                    new RectSpace(depositAreaTransform),
                    RectContainerElement.DragElementContainerBoundary.Edge);
                droppable.droppableRect.position -= (Vector3)boundaryOverlap;
            }
        }

        #endregion Drop Events
    }
}