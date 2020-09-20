using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GourdUI
{
    public partial class DragElement
    {
        #region Variables

        /// <summary>
        /// The deposit area we are actively hovering over (null if none)
        /// </summary>
        protected static DepositArea _activeDepositArea;

        /// <summary>
        /// 
        /// </summary>
        protected static Transform _originParent;

        /// <summary>
        /// 
        /// </summary>
        public DepositArea originalDepositArea { get; private set; }

        /// <summary>
        /// Cached event data
        /// </summary>
        private static PointerEventData _currentDropcastData;
        
        /// <summary>
        /// Cached raycast results list
        /// </summary>
        private static readonly List<RaycastResult> _dropcastResults = new List<RaycastResult>();

        /// <summary>
        /// 
        /// </summary>
        private Vector3 _defaultScale;

        #endregion Variables
        
        
        #region Raycast

        /// <summary>
        /// Uses event system to raycast to all UI elements as the elemnt is being dragged.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DropcastHover()
        {
            _originParent = dynamicTransform.parent;
            originalDepositArea = _activeDepositArea;
            
            while (true)
            {
                // Raycast with even system
                _currentDropcastData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };
                
                EventSystem.current.RaycastAll(_currentDropcastData, _dropcastResults);

                // Act on raycast results
                if (_dropcastResults.Count == 0 && _activeDepositArea != null)
                {
                    OnNullDropcast();
                }
                else
                {
                    foreach (RaycastResult result in _dropcastResults)
                    {
                        DepositArea d = result.gameObject.GetComponent<DepositArea>();
                        if (d != null)
                        {
                            if (d.gameObject != gameObject)
                            {
                                if (d != _activeDepositArea)
                                {
                                    if (_activeDepositArea != null)
                                    {
                                        _activeDepositArea.OnDroppableRaycastReceiveExit(this);
                                        OnDragElementContainerHoverExit(_activeDepositArea, d);
                                    }

                                    _activeDepositArea = d;
                                    _activeDepositArea.OnDroppableRaycastReceiveEnter(this);
                                    OnDragElementContainerHoverEnter(_activeDepositArea);
                                }
                                else
                                {
                                    _activeDepositArea.OnDroppableRaycastReceiveStay(this);
                                }
                            }
                        
                            break;
                        }
                        else
                        {
                            dynamicTransform.SetParent(result.gameObject.transform);
                            OnNullDropcast();
                        }
                    }
                }
                
                yield return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnNullDropcast()
        {
            if (_activeDepositArea != null)
            {
                _activeDepositArea.OnDroppableRaycastReceiveExit(this);
                OnDragElementContainerHoverExit(_activeDepositArea, null);
            }
            _activeDepositArea = null;
        }

        /// <summary>
        /// Checks to see if the draggable should be dropped into a container after drag has ended.
        /// </summary>
        private void Dropcast()
        {
            OnDragElementDrop(_activeDepositArea);
            if (_activeDepositArea != null)
            {
                _activeDepositArea.OnDroppableDrop(this);
            }
            else
            {
                dynamicTransform.SetParent(_defaultParent);
            }
        }

        #endregion Racyast


        #region Drop

        public virtual void OnDragElementContainerHoverEnter(DepositArea container)
        {
            dynamicTransform.SetParent(container.depositAreaTransform);
            
            // Inform decorators
            foreach (IDropElementEventReceivable d in _decorators)
            {
                d.OnDragElementContainerHoverEnter(container);
            }
        }

        public virtual void OnDragElementContainerHoverExit(DepositArea outContainer, DepositArea inContainer)
        {
            dynamicTransform.SetParent(_defaultParent);
            dynamicTransform.localScale = _defaultScale;
            
            // Inform decorators
            foreach (IDropElementEventReceivable d in _decorators)
            {
                d.OnDragElementContainerHoverExit(outContainer, inContainer);
            }
        }

        public virtual void OnDragElementDrop(DepositArea container)
        {
            // Cancel any momentum we may have after dragging
            if (container != null && 
                _originParent != container.depositAreaTransform && 
                _rectIsSlidingWithMomentum)
            {
                _rectIsSlidingWithMomentum = false;
                StopCoroutine(nameof(OnMomentumSlideFrame));
            }
            
            // If we drop out of the area we were previously in, we need to inform that area
            if (originalDepositArea != null && _activeDepositArea != originalDepositArea)
            {
                originalDepositArea.OnDroppableRemove(this);
            }
            dynamicTransform.localScale = _defaultScale;
            
            // Inform decorators
            foreach (IDropElementEventReceivable d in _decorators)
            {
                d.OnDragElementDrop(container);
            }
        }

        #endregion Drop
    }
}