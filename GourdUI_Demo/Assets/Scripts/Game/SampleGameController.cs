using System;
using UnityEngine;

namespace Game
{
    public class SampleGameController : MonoBehaviour
    {
        #region Variables

        public UIScreen_SampleHUD hudUI;

        public float _currentHealth = 50;
        public int _currentCoins = 0;

        #endregion Variables
        
        
        #region Singleton

        public static SampleGameController Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion Singleton
        

        public void OnCurrentHealthUpdated(float increment)
        {
            float oldHealth = _currentHealth;
            _currentHealth += increment;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, 100);
            if (oldHealth == _currentHealth)
                return;
            
            hudUI.OnHealthUpdated(oldHealth, _currentHealth); 
        }
        
        public void OnCurrentCoinsUpdated(int increment)
        {
            hudUI.OnCoinsUpdated(_currentCoins, _currentCoins + increment);
            _currentCoins += increment;
        }
    }
}