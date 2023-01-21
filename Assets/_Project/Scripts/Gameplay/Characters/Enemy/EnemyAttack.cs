using _Project.Scripts.Gameplay.Characters.Common;
using Attributes;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters.Enemy
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

        void Update()
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
            characterAnimator.SetTrigger(characterAttackParamName);
        }
        
        private bool IsPlayerInAttackRange()
        {
            return Physics.CheckSphere(transform.position, attackRange, chasePlayerScript.PlayerLayerMask);
        }
    }
}
