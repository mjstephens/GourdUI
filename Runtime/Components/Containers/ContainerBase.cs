using System.Collections.Generic;

namespace GourdUI
{
    // T defines the type this container can accept
    public abstract class ContainerBase<T>: DepositArea where T : class, IUIDroppable
    {
        #region Variables
        
        /// <summary>
        /// The boundary that defines the borders of this container.
        /// </summary>
        private RectSpace _boundary;
        
        #endregion Variables


        #region Initialization

        private void OnEnable()
        {
            UpdateContainerBoundary();
        }

        #endregion Initialization
        

        #region Boundary

        /// <summary>
        /// Updates the rect space representing the boundary of this container
        /// </summary>
        private void UpdateContainerBoundary()
        {
            _boundary = new RectSpace(depositAreaTransform);
            RefreshContainerBoundary(_boundary, _currentItems);
        }

        protected abstract void RefreshContainerBoundary(RectSpace boundary, List<IUIDroppable> items);

        #endregion Boundary
    }
}