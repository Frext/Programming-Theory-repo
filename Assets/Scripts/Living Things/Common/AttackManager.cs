using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Living_Things.Common
{
    public abstract class AttackManager : MonoBehaviour
    {
        [Header("Weapon Properties")]
        [SerializeField] protected Animator weaponAnimator;
        [SerializeField] protected Collider weaponCollider;

        [Header("Attack Properties")]
        [Range(0, 5)]
        [SerializeField] protected float attackCooldown;
        
        protected void Start()
        {
            // Disable the collider at the beginning to avoid attacking all the time.
            DisableAttacking();
        }

        protected abstract void Attack();

        protected abstract void PlayAttackAnimation();

        public bool DealDamage(GameObject callerObject, GameObject objectToDealDamage, int damageAmount)
        {
            if (objectToDealDamage != callerObject && objectToDealDamage != null)
            {
                HealthManager healthManager = objectToDealDamage.GetComponent<HealthManager>();
                
                if (healthManager != null)
                {
                    healthManager.GetDamage(damageAmount);

                    return true;
                }
            }

            return false;
        }

        protected void EnableAttacking()
        {
            weaponCollider.enabled = true;
        }

        protected void DisableAttacking()
        {
            weaponCollider.enabled = false;
        }
    }
}
