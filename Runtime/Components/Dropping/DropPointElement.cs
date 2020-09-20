using UnityEngine;

namespace GourdUI
{
    public class DropPointElement : DepositArea
    {
        #region Variables

        [Header("Settings")]
        [SerializeField]
        private bool _snapDroppablesToCenterOnDrop = true;

        #endregion Variables
        
        
        #region Drop Events

        public override void OnDroppableDrop(IUIDroppable droppable)
        {
            base.OnDroppableDrop(droppable);
            
            if (_snapDroppablesToCenterOnDrop)
            {
                droppable.droppableRect.position = depositAreaTransform.position;
            }
        }

        #endregion Drop Events
    }
}