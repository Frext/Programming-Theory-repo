using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Living_Things.Common
{
    // A collider is required for the object the health manager is in,
    // because the swords get the health manager reference from the collided object. 
    [RequireComponent(typeof(Collider))]
    public class HealthManager : MonoBehaviour
    {
        [SerializeField] private int initialHealth;
        [SerializeField] private Image healthBar;

        [Space] 
        [SerializeField] private UnityEvent onDie;

        // ENCAPSULATION
        private int _health;

        private int Health
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
            healthBar.fillAmount = (float)Health / initialHealth;
        }

        void Start()
        {
            Health = initialHealth;
        }

        public void GetDamage(int damageAmount)
        {
            Health -= damageAmount;

            InvokeOnDieEvent();
        }

        private void InvokeOnDieEvent()
        {
            if (Health == 0)
            {
                onDie.Invoke();
            }
        }
    }
}