using System;
using _Project.Scripts.Gameplay.Characters.Common;
using _Project.Scripts.Gameplay.Data.Scene.Scriptable_Object_Templates;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters.Player
{
    public class PlayerHealth : HealthManager
    {
        // This is used for nothing, but can stay here.
        [SerializeField] private IntObject playerHealthSO;

        protected override void Start()
        {
            base.Start();
            
            UpdatePlayerHealthSO();
        }

        private void UpdatePlayerHealthSO()
        {
            if(playerHealthSO != null)
                playerHealthSO.runtimeValue = Health;
        }

        #region Methods Used By Other Scripts
        
        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            
            UpdatePlayerHealthSO();
        }
        
        public bool IncreaseHealth(int amount)
        {
            if (amount > 0 && Health < maxHealth)
            {
                Health += amount;

                return true;
            }

            return false;
        }
        
        public void ColliderDie(GameObject colliderParent)
        {
            // Disable the colliders for the enemies not to raycast dead player presence.
            foreach (Collider childCollider in colliderParent.GetComponents<Collider>())
            {
                childCollider.enabled = false;
            }
        }
        
        #endregion
    }
}
