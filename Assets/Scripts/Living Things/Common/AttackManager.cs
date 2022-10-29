using UnityEngine;

namespace Living_Things.Common
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
                HealthManager healthManager = objectToDealDamage.GetComponent<HealthManager>();
                
                if (healthManager != null)
                {
                    print("Attacker : " + callerObject.name + " Victim : " + objectToDealDamage.name);
                    print(healthManager.gameObject.name);
                    
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
