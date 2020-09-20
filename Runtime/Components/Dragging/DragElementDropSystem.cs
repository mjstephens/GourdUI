using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GourdUI
{
    public partial class DragElement
    {
        #region Variables

        private static DepositArea _activeDepositArea;
        
        /// <summary>
        /// Cached event data
        /// </summary>
        private static PointerEventData _currentDropcastData;
        
        /// <summary>
        /// Cached raycast results list
        /// </summary>
        private static readonly List<RaycastResult> _dropcastResults = new List<RaycastResult>();

        #endregion Variables
        
        
        #region Raycast

        private IEnumerator DropcastHover()
        {
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
                    _activeDepositArea.OnDroppableRaycastReceiveExit(this);
                    OnDragElementContainerHoverExit(_activeDepositArea);
                    _activeDepositArea = null;
                }
                else
                {
                    foreach (RaycastResult result in _dropcastResults)
                    {
                        DepositArea d = result.gameObject.GetComponent<DepositArea>();
                        if (d != null && d.gameObject != gameObject)
                        {
                            if (d != _activeDepositArea)
                            {
                                if (_activeDepositArea != null)
                                {
                                    _activeDepositArea.OnDroppableRaycastReceiveExit(this);
                                }

                                _activeDepositArea = d;
                                _activeDepositArea.OnDroppableRaycastReceiveEnter(this);
                                OnDragElementContainerHoverEnter(_activeDepositArea);
                            }
                            else
                            {
                                _activeDepositArea.OnDroppableRaycastReceiveStay(this);
                            }
                        
                            break;
                        }
                    }
                }
                
                yield return null;
            }
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
        }

        #endregion Racyast


        #region Drop

        public void OnDragElementContainerHoverEnter(DepositArea container)
        {
            dynamicTransform.SetParent(container.depositAreaTransform);
        }

        public void OnDragElementContainerHoverExit(DepositArea container)
        {
            dynamicTransform.SetParent(_defaultParent);
        }

        public void OnDragElementDrop(DepositArea container)
        {
            if (container != null)
            {
                dynamicTransform.SetParent(container.depositAreaTransform);
            }
            else
            {
                dynamicTransform.SetParent(_defaultParent);
            }
            // if (dropZone.snapType == DropZoneElement.SnapOptions.None && !_parentOnDrop)
            //     return;
            //
            // switch (dropZone.snapType)
            // {
            //     case DropZoneElement.SnapOptions.ToCenter:
            //         dynamicTransform.position = dropZone.transform.position;
            //         break;
            //     case DropZoneElement.SnapOptions.ToContained:
            //         Vector2 boundaryOverlap = RectBoundariesUtility.GetRectSpaceOverlap(
            //             new RectSpace(dynamicTransform),
            //             new RectSpace(dropZone.dynamicTransform),
            //             RectContainerElement.DragElementContainerBoundary.Edge);
            //         dynamicTransform.position -= (Vector3)boundaryOverlap;
            //         break;
            // }
            //
            // if (_parentOnDrop)
            // {
            //     transform.SetParent(dropZone.transform);
            // }
            //
            // if (_rectIsSlidingWithMomentum)
            // {
            //     _rectIsSlidingWithMomentum = false;
            //     StopCoroutine(nameof(OnMomentumSlideFrame));
            // }
        }

        #endregion Drop
    }
}