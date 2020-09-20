using System.Collections.Generic;
using UnityEngine;

namespace GourdUI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class DepositArea : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        #region Variables

        /// <summary>
        /// Contains the items (ordered) currently in this container
        /// </summary>
        protected readonly List<IUIDroppable> _currentItems = new List<IUIDroppable>();

        /// <summary>
        /// The transform of this deposit area object
        /// </summary>
        public RectTransform depositAreaTransform { get; private set; }
        
        #endregion Variables


        #region Initialization

        private void Awake()
        {
            depositAreaTransform = GetComponent<RectTransform>();
        }

        #endregion Initialization
        
        
        #region Drop Events

        public abstract void OnDroppableRaycastReceiveEnter(IUIDroppable droppable);

        public abstract void OnDroppableRaycastReceiveStay(IUIDroppable droppable);

        public abstract void OnDroppableRaycastReceiveExit(IUIDroppable droppable);

        public abstract void OnDroppableDrop(IUIDroppable droppable);

        #endregion Drop Events
    }
}