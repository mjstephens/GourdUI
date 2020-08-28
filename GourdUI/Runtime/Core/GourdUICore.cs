using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GourdUI
{
    public class GourdUICore : IGourdUI, IUIInputListener
    {
        #region Variables

        private static bool _locked;
        
        private readonly GourdUISystemData _systemData;
        private readonly List<UIScreen> _currentUIScreens;
        private readonly List<UIScreen> _screenStack;
        //private readonly List<UIScreenTriggerBase> _triggers;
        
        #endregion Variables


        #region Constructor

        public GourdUICore(GourdUISystemData data)
        {
            // Cache system data
            _systemData = data;
            
            // Create lists
            _currentUIScreens = new List<UIScreen>();
            _screenStack = new List<UIScreen>();
            
            // Create input module
            //_inputTriggerListener = new UIInputTriggerListener(_systemData.uiTriggerInputAsset);
        }

        #endregion Constructor
        
        
        #region Registration

        void IGourdUI.RegisterScreen(UIScreen screen)
        {
            _currentUIScreens.Add(screen);
            screen.OnScreenInstantiated();

// #if ENABLE_INPUT_SYSTEM
//             foreach (var trigg in screen.configBaseData.data.triggerActions)
//             {
//                 _inputTriggerListener.SubscribeActionListener(this, trigg);
//             }
// #endif
// #if ENABLE_LEGACY_INPUT_MANAGER
//
// #endif
        }
        
        void IGourdUI.UnregisterScreen(UIScreen screen)
        {
            // Remove from stack first
            if (_screenStack.Contains(screen))
            {
                RemoveScreenFromStack(screen);
            }
            _currentUIScreens.Remove(screen);
            
            // InputActionReference r = screen.configBaseData.data.triggerAction;
            // if (r != null)
            // {
            //     _inputTriggerListener.UnubscribeActionListener(this, r);
            // }
        }

        #endregion Registration
        
        
        #region Stack

        public void AddScreenToStack<T>(UIScreen screen, T data = default)
        {
            // If we are waiting for input, we should not be able to add more UI
            if (_locked)
                return;

            // Remove first, then re-add
            _screenStack.Remove(screen);
            _screenStack.Add(screen);
            
            // Enable the UI screen
            screen.OnScreenEnabled(data);

            // Refresh the stack
            RefreshUIScreenStack();
            
            // If this new UI screen requires action to continue, flag it
            if (screen.configBaseData.data.lockUIUntilDismissed)
            {
                _locked = true;
            }
        }
        
        public void RemoveScreenFromStack(UIScreen screen)
        {
            _screenStack.Remove(screen);
            screen.OnScreenDisabled();
            if (screen.configBaseData.data.lockUIUntilDismissed)
            {
                _locked = false;
            }

            // Refresh the stack
            RefreshUIScreenStack();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        private void ToggleUIScreen<T>(UIScreen screen, T data = default)
        {
            if (!_screenStack.Contains(screen))
            {
                AddScreenToStack(screen, data);
            }
            else
            {
                RemoveScreenFromStack(screen);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshUIScreenStack()
        {
            // Does the UI need control?
            bool uiIsActive = false;
            UIScreen firstActiveScreen = null;
            for (int i = _screenStack.Count - 1; i >= 0; i--)
            {
                UIScreen screen = _screenStack[i];
                screen.OnScreenSetStackOrder(_systemData.canvasRenderDepthBase + i);
                if (screen.configBaseData.data.screenType == UIScreenConfigData.ScreenType.Active && !uiIsActive)
                {
                    uiIsActive = true;
                    firstActiveScreen = screen;
                }
            }

            // If the UI is active, that means it currently has control over the game
            Time.timeScale = 1;
            if (uiIsActive)
            {
                UICursorController.UpdateCursor(true);
                if (firstActiveScreen != null)
                {
                    // Some active UI screens may freeze the simulation while they wait for input
                    if (firstActiveScreen.configBaseData.data.freezeSimulation)
                    {
                        Time.timeScale = 0;
                    }
                }
            }
        }

        #endregion Stack


        #region View
        
        void IGourdUI.OnAppDeviceUpdated(AppDeviceData deviceData)
        {
            foreach (var screen in _currentUIScreens)
            {
                screen.OnAppDeviceDataUpdated(deviceData);
            }
        }

        bool IGourdUI.UIViewIsValidForDevice(UIViewConfigData viewData, AppDeviceData deviceData)
        {
            return UIViewFilterResolver.ViewFilterResult(viewData.filterData, deviceData);
        }

        #endregion View
        
        
        #region Input

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        void IUIInputListener.OnUIInputAction(InputAction.CallbackContext ctx)
        {
            // Only trigger UI on "performed" action phase
            if (ctx.action.phase == InputActionPhase.Performed)
            {
                ToggleUIScreenFromInput(ctx.action);
            }            
        }

        #endregion Input
        
        
        #region Toggle
        
        void IGourdUI.ToggleUIScreen(string screenTriggerCode)
        {
            UIScreen s = null;
            for (int i = _currentUIScreens.Count - 1; i >= 0; i--)
            {
                if (_currentUIScreens[i].configBaseData.data.triggerCode == screenTriggerCode)
                {
                    s = _currentUIScreens[i];
                    break;
                }
            }
            
            // If we found a screen for the action, toggle it
            if (s != null)
            {
                ToggleUIScreen(s, 0);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        private void ToggleUIScreenFromInput(InputAction action)
        {
            UIScreen s = null;
            // for (int i = _currentUIScreens.Count - 1; i >= 0; i--)
            // {
            //     if (_currentUIScreens[i].configBaseData.data.triggerAction != null)
            //     {
            //         if (_currentUIScreens[i].configBaseData.data.triggerAction.action == action)
            //         {
            //             s = _currentUIScreens[i];
            //             break;
            //         }
            //     }
            // }
            
            // If we found a screen for the action, toggle it
            if (s != null)
            {
                ToggleUIScreen(s, 0);
            }
        }
        
        #endregion Toggle
    }
}