namespace GourdUI
{
    public partial class DragElement
    {
        #region State

        public void ApplyState(DragElementState state)
        {
            _interacting = false;
            dynamicTransform.localPosition = state.dtPosition;
            //dynamicTransform.anchoredPosition = state.dtAnchoredPosition;
            _offset = state.offset;
            _dragger.position = state.draggerPosition;
            _draggerPrevPos = state.draggerPreviousPosition;
            _releaseMomentum = state.releaseMomentum;
            _xAxisBoundaryReached = state.xBoundaryReached;
            _yAxisBoundaryReached = state.yBoundaryReached;
            _rectIsSlidingWithMomentum = state.sliding;
            activeControl = state.activeControl;
            
            // If we were sliding, resume the movement
            if (activeControl && _rectIsSlidingWithMomentum)
            {
                StartCoroutine(nameof(OnMomentumSlideFrame));
            }
            else
            {
                base.UpdateDynamicRect();
            }
        }

        public DragElementState RetrieveState()
        {
            return new DragElementState
            {
                activeControl = activeControl,
                dtPosition = dynamicTransform.localPosition,
                dtAnchoredPosition = dynamicTransform.anchoredPosition,
                offset = _offset,
                draggerPosition = _dragger.position,
                draggerPreviousPosition = _draggerPrevPos,
                releaseMomentum = _releaseMomentum,
                xBoundaryReached = _xAxisBoundaryReached,
                yBoundaryReached = _yAxisBoundaryReached,
                sliding = _rectIsSlidingWithMomentum
            };
        }

        #endregion
    }
}