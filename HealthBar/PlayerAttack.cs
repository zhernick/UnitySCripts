using System;
using Resources.Scripts.Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour, IDamagable
    {
        private HashAnimations animBase = new HashAnimations();
        public bool attacking;
        private bool holdingAttackButton;
        private Animator _anim;
        [SerializeField] private int playerHealth;
        [SerializeField] private int playerCurrentHealth;
        [SerializeField] private int playerAttack;
        private int baseHeath = 100;
        private int baseAttack = 100;

        [SerializeField] private HealthBar healthBar;
        [SerializeField] private bool smoothHealth;

        public static PlayerAttack Instance;

        private void Awake()
        {
            Instance = this;
            _anim = GetComponent<Animator>();
        }

        private void Start()
        {
            Init();
        }

        void Update()
        {
            Attack();

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

        public int GetHealth()
        {
            return playerHealth;
        }
        public int GetAttack()
        {
            return playerAttack;
        }

        private void CheckStats()
        {
            if (playerAttack < baseAttack) playerAttack = baseAttack;
            if (playerHealth < baseHeath) playerHealth = baseHeath;
        }

        public void AddStats(int health, int attack)
        {
            playerHealth += health;
            playerAttack += attack;
            CheckStats();
        }
        
        public void ReduceStats(int health, int attack)
        {
            playerHealth -= health;
            playerAttack -= attack;
            CheckStats();
        }

        void Attack()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                holdingAttackButton = true;
            }else if (Input.GetKeyUp(KeyCode.E))
            {
                holdingAttackButton = false;
            }
            
            if (holdingAttackButton)
            {
                // Когда атакую
                attacking = true;
                _anim.CrossFade(animBase.Attack1, 0);
            }
        }

        // не лезть сюда
        public void JoystickAttack()
        {
            holdingAttackButton = true;
        }
        public void JoystickAttackStop()
        {
            holdingAttackButton = false;
        }
        public void ExitingAttack()
        {
            _anim.CrossFade(animBase.Idle, 0);
            attacking = false;
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
