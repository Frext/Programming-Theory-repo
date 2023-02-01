using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay.Characters.Common
{
    // A collider is required for the object the health manager is in,
    // because the swords get the health manager reference from the collided object. 
    [RequireComponent(typeof(Collider))]
    public abstract class HealthManager : MonoBehaviour
    {
        [SerializeField] protected int maxHealth;
        [SerializeField] protected Image healthBar;

        [Space]
        [SerializeField] protected UnityEvent onTakeDamage;
        [SerializeField] protected UnityEvent onDie;

        
        // ENCAPSULATION
        private int _health;
        protected int Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, int.MaxValue);

                UpdateHealthBar();
            }
        }

        private void UpdateHealthBar()
        {
            if (healthBar != null)
            {
                healthBar.fillAmount = (float)Health / maxHealth;
            }
        }
        

        protected virtual void Start()
        {
            SetHealthToInitial();
        }

        private void SetHealthToInitial()
        {
            Health = maxHealth;
        }

        #region Methods Used By Other Scripts

        public void TakeDamage(int damageAmount)
        {
            Health -= damageAmount;

            InvokeEvents();
        }

        private void InvokeEvents()
        {
            if (Health == 0)
            {
                onDie.Invoke();
            }
            else
            {
                onTakeDamage.Invoke();
            }
        }

        #endregion
    }
}