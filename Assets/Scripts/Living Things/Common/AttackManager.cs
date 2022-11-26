using UnityEngine;

namespace Living_Things.Common
{
    public abstract class AttackManager : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected Collider weaponCollider;

        [Space]
        [Range(0, 5)]
        [SerializeField] protected float attackCooldown;


        protected abstract void Attack();

        protected abstract void PlayAttackAnimation();

        public void DealDamage(GameObject callerObject,GameObject objectToDealDamage,int damageAmount)
        {
            if (objectToDealDamage != callerObject && objectToDealDamage != null)
            {
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
