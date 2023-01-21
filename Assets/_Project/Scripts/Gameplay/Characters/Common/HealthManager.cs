using Unity.VisualScripting;
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
            healthBar.fillAmount = (float)Health / initialHealth;
        }

        void OnEnable()
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
            if(!enabled)
                return;
            
            if (Health == 0)
            {
                onDie.Invoke();

                // Disable this script to avoid invoking the on die event more than once.
                enabled = false;
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