using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace GourdUI
{
    public class UIInputTriggerListener
    {
        #region Variables

        /// <summary>
        /// List of current input action listeners and their target actions.
        /// </summary>
        private readonly List<Tuple<IUIInputListener, InputActionReference>> _listeners;

        #endregion Variables
        
        
        #region Constructor

        public UIInputTriggerListener(InputActionAsset actionAsset)
        {
            // Create input listener list
            _listeners = new List<Tuple<IUIInputListener, InputActionReference>>();
            
            // Create listeners for all action maps
            if (actionAsset != null)
            {
                actionAsset.Enable();
                foreach (var map in actionAsset.actionMaps)
                {
                    map.actionTriggered += OnActionMapTriggered;
                }
            }
        }

        #endregion Contructor
        
        
        #region Callbacks

        /// <summary>
        /// Listens for inpt actions from all action maps.
        /// </summary>
        /// <param name="ctx"></param>
        private void OnActionMapTriggered(InputAction.CallbackContext ctx)
        {
            if (ctx.action != null)
            {
                foreach (var (listener, reference) in _listeners)
                {
                    if (ctx.action.name == reference.action.name)
                    {
                        listener.OnUIInputAction(ctx);
                    }
                }
            }
        }

        #endregion Callbacks
        
        
        #region Listeners

        /// <summary>
        /// Subscribes a listener.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="action"></param>
        public void SubscribeActionListener(
            IUIInputListener listener, 
            InputActionReference action)
        {
            _listeners.Add(new Tuple<IUIInputListener, InputActionReference>(listener, action));
        }
        
        
        /// <summary>
        /// Unsubscribes a listener
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="action"></param>
        public void UnubscribeActionListener(
            IUIInputListener listener, 
            InputActionReference action)
        {
            _listeners.Remove(new Tuple<IUIInputListener, InputActionReference>(listener, action));
        }

        #endregion Listeners
    }
}