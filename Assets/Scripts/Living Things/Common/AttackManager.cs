using System;
using UnityEngine;

namespace Living_Things
{
    public abstract class AttackManager : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected Collider weaponCollider;

        [Space]
        [SerializeField] protected float attackCooldown;


        protected abstract void Attack();

        protected abstract void PlayAttackAnimation();

        public void DealDamage(GameObject callerObject,GameObject objectToDealDamage,int damageAmount)
        {
            if (objectToDealDamage != callerObject && objectToDealDamage != null)
            {
                print("Attacker : " + callerObject.name + "Victim : " + objectToDealDamage.name);

                HealthManager healthManager = objectToDealDamage.GetComponent<HealthManager>();
                
                if (healthManager != null)
                {
                    healthManager.GetDamage(damageAmount);
                }
            }
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
