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
        
        static readonly int Attack1 = Animator.StringToHash("attack");
        
        void Start()
        {
            DisableAttacking();
        }

        void FixedUpdate()
        {
            if (IsPlayerInAttackRange())
            {
                Attack();
            }
        }

        protected override void Attack()
        {
            if (timePassed < Time.fixedTime)
            {
                PlayAttackAnimation();

                EnableAttacking();
                Invoke(nameof(DisableAttacking), attackCooldown / 2);

                timePassed = attackCooldown + Time.fixedTime;
            }
        }

        protected override void PlayAttackAnimation()
        {
            animator.SetTrigger(Attack1);
        }
        
        private bool IsPlayerInAttackRange()
        {
            return Physics.CheckSphere(transform.position, attackRange, playerLayerMask);
        }
    }
}
