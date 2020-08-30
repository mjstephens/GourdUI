using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GourdUI
{
    public class GourdUICore: IGourdUI, IUIInputListener
    {
        #region Variables

        private static bool _locked;
        
        private readonly GourdUISystemData _systemData;
        private readonly List<IUIScreen> _currentUIScreens;
        private readonly List<IUIScreen> _screenStack;
        
        #endregion Variables


        #region Constructor

        public GourdUICore(GourdUISystemData data)
        {
            // Cache system data
            _systemData = data;
            
            // Create lists
            _currentUIScreens = new List<IUIScreen>();
            _screenStack = new List<IUIScreen>();
            
            // Create input module
            //_inputTriggerListener = new UIInputTriggerListener(_systemData.uiTriggerInputAsset);
        }

        #endregion Constructor
        
        
        #region Registration
        
        void IGourdUI.RegisterScreen(IUIScreen screen)
        {
            _currentUIScreens.Add(screen);
            screen.OnScreenInstantiated();
        }

        void IGourdUI.UnregisterScreen(IUIScreen screen)
        {
            // Remove from stack first
            if (_screenStack.Contains(screen))
            {
                RemoveScreenFromStack(screen);
            }
            _currentUIScreens.Remove(screen);
        }

        #endregion Registration
        
        
        #region Stack

        public void AddScreenToStack<T>(IUIScreen screen, T data = default)
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
            if (screen.ScreenConfigData().lockUIUntilDismissed)
            {
                _locked = true;
            }
        }
        
        public void RemoveScreenFromStack(IUIScreen screen)
        {
            if (!_screenStack.Contains(screen))
                return;
            
            _screenStack.Remove(screen);
            screen.OnScreenDisabled();
            if (screen.ScreenConfigData().lockUIUntilDismissed)
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
        private void ToggleUIScreen<T>(IUIScreen screen, T data = default)
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
            IUIScreen firstActiveScreen = null;
            for (int i = _screenStack.Count - 1; i >= 0; i--)
            {
                IUIScreen screen = _screenStack[i];
                screen.OnScreenSetStackOrder(_systemData.canvasRenderDepthBase + i);
                if (screen.ScreenConfigData().screenType == 
                    UIScreenConfigData.ScreenType.Active && !uiIsActive)
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
                    if (firstActiveScreen.ScreenConfigData().freezeSimulation)
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
            IUIScreen s = null;
            for (int i = _currentUIScreens.Count - 1; i >= 0; i--)
            {
                if (_currentUIScreens[i].ScreenConfigData().triggerCode == screenTriggerCode)
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
            UIScreen<IUIViewContract, UIState> s = null;
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