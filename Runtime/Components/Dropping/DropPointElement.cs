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

        public override void OnDroppableRaycastReceiveEnter(IUIDroppable droppable)
        {
            
        }

        public override void OnDroppableRaycastReceiveStay(IUIDroppable droppable)
        {
            
        }

        public override void OnDroppableRaycastReceiveExit(IUIDroppable droppable)
        {
            
        }

        public override void OnDroppableDrop(IUIDroppable droppable)
        {
            if (_snapDroppablesToCenterOnDrop)
            {
                droppable.droppableRect.position = depositAreaTransform.position;
            }
        }

        #endregion Drop Events
    }
}