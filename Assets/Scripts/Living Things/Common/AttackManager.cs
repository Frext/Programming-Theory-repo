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

        protected virtual void DealDamage(GameObject objectToDealDamage,int damageAmount)
        {
            objectToDealDamage.GetComponent<HealthManager>().GetDamage(damageAmount);
        }
    }
}
