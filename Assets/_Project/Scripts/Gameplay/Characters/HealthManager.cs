using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay.Characters.Common
{
    // A collider is required for the object the health manager is in,
    // because the swords get the health manager reference from the collided object. 
    [RequireComponent(typeof(Collider))]
    public class HealthManager : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private Image healthBar;

        [Space] 
        [SerializeField] private UnityEvent onTakeDamage;
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
            if (healthBar != null)
            {
                healthBar.fillAmount = (float)Health / maxHealth;
            }
        }

        void Start()
        {
            SetHealthToInitial();
        }
        
        // This is also used when an enemy returns to the pool, so it must be public.
        public void SetHealthToInitial()
        {
            Health = maxHealth;
        }

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

        #region Player Methods
        
        public void ColliderDie(GameObject colliderParent)
        {
            // Disable the colliders for the enemies not to raycast dead player presence.
            foreach (Collider childCollider in colliderParent.GetComponents<Collider>())
            {
                childCollider.enabled = false;
            }
        }
        #endregion
        
        #region Pickup Methods
        
        public bool AddHealth(int amount)
        {
            if (amount > 0 && Health < maxHealth)
            {
                Health += amount;

                return true;
            }

            return false;
        }
        #endregion
    }
}