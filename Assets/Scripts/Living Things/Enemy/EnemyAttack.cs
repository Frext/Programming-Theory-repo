using Living_Things.Common;
using UnityEngine;

namespace Living_Things.Enemy
{
    public class EnemyAttack : AttackManager
    {
        [Header("Detect Player")]
        [SerializeField] private LayerMask playerLayerMask;
        
        [Header("Attack Properties")]
        [SerializeField] private float attackRange;
        
        
        float timePassed;

        
        void Start()
        {
            DisableAttacking();
        }

        void Update()
        {
            if (IsPlayerInAttackRange())
            {
                Attack();
            }
        }

        protected override void Attack()
        {
            if (timePassed < Time.time)
            {
                PlayAttackAnimation();

                EnableAttacking();
                Invoke(nameof(DisableAttacking), attackCooldown / 2);

                timePassed = attackCooldown + Time.time;
            }
        }

        protected override void PlayAttackAnimation()
        {
            animator.SetTrigger("attack");
        }
        
        private bool IsPlayerInAttackRange()
        {
            return Physics.CheckSphere(transform.position, attackRange, playerLayerMask);
        }
    }
}
