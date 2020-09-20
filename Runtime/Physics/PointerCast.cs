using System.Collections.Generic;
using GourdUI;

namespace Physics
{
    public static class PointerCast
    {
        #region Variables

        private static readonly List<DragElement> _activeDraggers = new List<DragElement>();
        private static readonly List<ContainerElement> _activeContainers = new List<ContainerElement>();
        
        #endregion Variables


        #region Subscribe

        public static void SubscribeDragElement(DragElement e)
        {
            _activeDraggers.Add(e);
        }

        public static void UnsubscribeDragElement(DragElement e)
        {
            _activeDraggers.Remove(e);
        }
        
        public static void SubscribeContainerElement(ContainerElement e)
        {
            _activeContainers.Add(e);
        }

        public static void UnsubscribeContainerElement(ContainerElement e)
        {
            _activeContainers.Remove(e);
        }

        #endregion Subscribe


        #region Update

        //public static void 
        
        #endregion Update
    }
}