using UnityEngine;

namespace GourdUI
{
    public interface IUIDynamicRect
    {
        #region Properties

        /// <summary>
        /// The rect transform being dynamically altered.
        /// </summary>
        RectTransform dynamicTransform { get; }
        
        /// <summary>
        /// 
        /// </summary>
        IUIDynamicRectFilter filter { set;  }
        
        /// <summary>
        /// 
        /// </summary>
        bool activeControl { get; }

        #endregion Properties


        #region Methods

        void SubscribeDynamicRectListener(IUIDynamicRectListener l);
        void UnsubscribeDynamicRectListener(IUIDynamicRectListener l);
        
        /// <summary>
        /// Listeners or filters may have need of forcing a dynamic rect update.
        /// </summary>
        void ForceUpdate();

        #endregion Methods
    }
}