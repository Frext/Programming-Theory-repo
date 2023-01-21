using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters.Common
{
    public abstract class AttackManager : MonoBehaviour
    {
        [Header("Weapon Properties")]
        [Tooltip("The animator of the character is needed for playing the attacking animation.")]
        [SerializeField] protected Animator characterAnimator;
        [SerializeField] protected string characterAttackParamName;
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

        public bool DealDamage(GameObject attackerObject, GameObject objectToDealDamage, int damageAmount)
        {
            if (objectToDealDamage != attackerObject && objectToDealDamage != null)
            {
                HealthManager healthManager = objectToDealDamage.GetComponent<HealthManager>();
                
                if (healthManager != null)
                {
                    healthManager.TakeDamage(damageAmount);

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
