using GourdUI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.MainMenu.Scripts
{
    public class UIView_MainMenu : MonoUIView, IUIContract_MainMenu
    {
        #region Variables

        [SerializeField] 
        private Button _openDemoShopButton;

        #endregion Variables


        #region Contract

        Button IUIContract_MainMenu.OpenDemoShopButton()
        {
            return _openDemoShopButton;
        }

        #endregion Contract
    }
}