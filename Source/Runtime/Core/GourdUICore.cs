using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GourdUI
{
    /// <summary>
    /// Main controller class for GourdUI. Holds references to screens, facilitates the screen stack, and
    /// responds to device changes/input triggers.
    /// </summary>
    public class GourdUICore: IGourdUI, IUIInputListener
    {
        #region Variables
        
        /// <summary>
        /// Generic system data
        /// Located at: GourdUI.coreUIDataPath (resources folder)
        /// </summary>
       // private readonly GourdUISystemData _systemData;
        
        /// <summary>
        /// The list of all UIScreens that are currently known to the core
        /// </summary>
        private readonly List<IUIScreen> _currentUIScreens;
        
        /// <summary>
        /// The list of UIScreens that are currently in the screen stack
        /// Stack is ORDERED; highest screens are first in the list, lower screens are further towards end
        /// </summary>
        private readonly List<IUIScreen> _screenStack;
        
        /// <summary>
        /// Screens can optionally "lock" the stack to prevent further action until they are dismissed
        /// </summary>
        private static bool _locked;
        
        #endregion Variables


        #region Constructor

        public GourdUICore()
        {
            // Cache system data
            //_systemData = data;
            
            // Create lists
            _currentUIScreens = new List<IUIScreen>();
            _screenStack = new List<IUIScreen>();
        }

        #endregion Constructor
        
        
        #region Registration
        
        // Registering a screen allows it to be used by the core; should happen only once per screen
        void IGourdUI.RegisterScreen(IUIScreen screen)
        {
            _currentUIScreens.Add(screen);
            screen.OnScreenInstantiated();
        }

        // Removes the screen from the core
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

        // Equivalent to enabling/activating a screen
        public void AddScreenToStack<T>(IUIScreen screen, T data = default)
        {
            if (_locked)
                return;

            // Move screen to top of stack
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
        
        // Equivalent to deactivating/disabling a screen
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
                screen.OnScreenSetStackOrder(i);
                if (screen.ScreenConfigData().screenType == 
                    UIScreenConfigData.ScreenType.Active && !uiIsActive)
                {
                    uiIsActive = true;
                    firstActiveScreen = screen;
                }
            }

            //TODO: Fix null check here, update sim freeze/cursor functionality for recent refactors
            
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
            return UIViewFilterResolver.ViewFilterResult(
                viewData.filterData.positiveFilters.ToArray(),
                viewData.filterData.negativeFilters.ToArray(),
                deviceData);
        }

        #endregion View
        
        
        #region Toggle
        
        //
        // TODO: Reintroduce input/toggle system
        //
        
        void IUIInputListener.OnUIInputAction(InputAction.CallbackContext ctx)
        {
            // Only trigger UI on "performed" action phase
            if (ctx.action.phase == InputActionPhase.Performed)
            {
                //ToggleUIScreenFromInput(ctx.action);
            }            
        }
        
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

        // private void ToggleUIScreenFromInput(InputAction action)
        // {
        //     UIScreen<IScreenUpdateReceivable, UIState> s = null;
        //     for (int i = _currentUIScreens.Count - 1; i >= 0; i--)
        //     {
        //         if (_currentUIScreens[i].configBaseData.data.triggerAction != null)
        //         {
        //             if (_currentUIScreens[i].configBaseData.data.triggerAction.action == action)
        //             {
        //                 s = _currentUIScreens[i];
        //                 break;
        //             }
        //         }
        //     }
        //     
        //     // If we found a screen for the action, toggle it
        //     if (s != null)
        //     {
        //         ToggleUIScreen(s, 0);
        //     }
        // }
        
        #endregion Toggle
    }
}