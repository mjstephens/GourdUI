using GourdUI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.MainMenu.Scripts
{
    public class UIView_MainMenu : UIView<IUIContract_MainMenu>, IUIContract_MainMenu
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

        public override void OnViewPreSetup()
        {
            
        }

        // public override void OnStateDataUpdated<T>(T stateData)
        // {
        //     
        // }
    }
}