using System;
using _Project.Scripts.Gameplay.Managers;
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

        [HideInInspector] public GameObjectPool ownerPoolScript;

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
            SetHealthToInitial();
        }
        
        // This is also used when an enemy returns to the pool, so it must be public.
        public void SetHealthToInitial()
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

        #region Enemy Methods
        
        public void ReturnToOwnerPool(GameObject parentGameObject)
        {
            if (ownerPoolScript != null)
                ownerPoolScript.AddToQueue(parentGameObject);
            else
                throw new Exception("Can't add an object without an owner pool to queue.");
        }
        #endregion
    }
}