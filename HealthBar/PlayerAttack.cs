using System;
using Resources.Scripts.Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private int playerHealth;
        [SerializeField] private int playerCurrentHealth;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private bool smoothHealth;

        private void Start()
        {
            Init();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                TakeDamage(10);
            }
            
            healthBar.SmoothSliderHealthMovement(playerCurrentHealth, ref smoothHealth);
        }

        private void Init()
        {
            playerHealth = baseHeath;
            playerAttack = baseAttack;
            playerCurrentHealth = playerHealth;
            
            healthBar.SetHealth(playerHealth, playerCurrentHealth);
        }

        public void TakeDamage(int damage)
        {
            playerCurrentHealth -= damage;
            
            // Логика для медленного уменьшения хп
            healthBar.UpdateSliderValues(playerHealth, playerCurrentHealth);
            healthBar.ResetSmoothTimer(false);
            smoothHealth = true;
        }
    }
}
