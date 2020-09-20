using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GourdUI
{
    
    
    
    
    // UNUSED - DELETE!!!!!
    
    
    
    
    
    
    
    public class DropZoneElement : UIDynamicElement, IPointerEnterHandler, IPointerExitHandler
    {
        #region Enum

        public enum SnapOptions
        {
            None,
            ToCenter,
            ToContained
        }

        #endregion Enum
        
        
        #region Variables

        [Header("Snapping")]
        public SnapOptions snapType;
        
        private List<IUIDroppable> _hoveringDroppables = new List<IUIDroppable>();
        private int _enterCount;

        #endregion Variables


        #region Initialization

        private void Awake()
        {
            dynamicTransform = transform.GetComponent<RectTransform>();
        }

        #endregion Initialization
        
        
        #region Pointer Events

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _enterCount = DragElement.activeDragElements.Count;
            if (_enterCount > 0 && !activeControl)
            {
                // foreach (IUIDroppable dragElement in DragElement.activeDragElements)
                // {
                //     //dragElement.OnDragElementContainerHoverEnter(this);
                // }
                //
                // // Check to see if we drop inside this droppable
                // _hoveringDroppables = new List<IUIDroppable>(DragElement.activeDragElements);
                // Debug.Log("ENTER");
                // ActivateElementInteraction();
            }
        }
        
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            EndElementInteraction();

            if (_hoveringDroppables.Count > 0)
            {
               // Debug.Log("EXIT");

                if (_enterCount != DragElement.activeDragElements.Count)
                {
                    foreach (IUIDroppable dragElement in _hoveringDroppables)
                    {
                        //dragElement.OnDragElementDrop(this);
                    }
                }
                else
                {
                    foreach (IUIDroppable dragElement in _hoveringDroppables)
                    {
                        //dragElement.OnDragElementContainerHoverExit(this);
                    }
                }
            }
            
            _hoveringDroppables.Clear();
        }

        #endregion Pointer Events


        #region Drop

        protected override void InteractionTick(Vector2 activeInputPosition) { }

        #endregion Drop
    }
}