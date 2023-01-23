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
        [SerializeField] private int initialHealth;
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
                healthBar.fillAmount = (float)Health / initialHealth;
            }
        }

        void Start()
        {
            SetHealthToMax();
        }
        
        // It's used when a pool object gets added to pool again. We need to set the health to the initial value again.
        public void SetHealthToMax()
        {
            Health = initialHealth;
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

        public void ColliderDie(GameObject colliderParent)
        {
            // Disable the colliders for the enemies not to raycast dead player presence.
            foreach (Collider childCollider in colliderParent.GetComponents<Collider>())
            {
                childCollider.enabled = false;
            }
        }
    }
}