using Attributes;
using Living_Things.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace Living_Things.Enemy
{
    public class EnemyAttack : AttackManager
    {
        [Range(0, 50)]
        [SerializeField] private float attackRange;
        
        public bool isAttackArea;
        [ConditionalHide(nameof(isAttackArea))]
        public float attackDelay;
        
        
        [Header("Detect Player")]
        [Tooltip("This script is used for getting the layer mask of player.")]
        [SerializeField] private ChasePlayer chasePlayerScript;
        
        
        float timePassed;
        
        static readonly int Attack1 = Animator.StringToHash("attack");

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

                Invoke(nameof(EnableAttacking), attackDelay);
                Invoke(nameof(DisableAttacking), attackCooldown / 2);

                timePassed = attackCooldown + Time.fixedTime;
            }
        }

        protected override void PlayAttackAnimation()
        {
            weaponAnimator.SetTrigger(Attack1);
        }
        
        private bool IsPlayerInAttackRange()
        {
            return Physics.CheckSphere(transform.position, attackRange, chasePlayerScript.PlayerLayerMask);
        }
    }
}
