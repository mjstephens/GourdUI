using UnityEngine;

namespace GourdUI
{
    public interface IUIDynamicElement
    {
        #region Properties

        /// <summary>
        /// The rect transform being dynamically altered.
        /// </summary>
        RectTransform dynamicTransform { get; }

        /// <summary>
        /// 
        /// </summary>
        bool activeControl { get; }

        #endregion Properties


        #region Methods

        void SubscribeDynamicElementListener(IUIDynamicElementListener l);
        void UnsubscribeDynamicElementListener(IUIDynamicElementListener l);
        
        /// <summary>
        /// Listeners or filters may have need of forcing a dynamic rect update.
        /// </summary>
        void ForceUpdate();

        #endregion Methods
    }
}