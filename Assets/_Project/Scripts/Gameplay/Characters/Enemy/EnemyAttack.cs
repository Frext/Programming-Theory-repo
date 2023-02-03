using _Project.Scripts.Gameplay.Characters.Common;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters.Enemy
{
    public class EnemyAttack : AttackManager
    {
        [Space]
        [Range(0, 10)] [SerializeField] private float attackRange;

        [Header("Detect Player")]
        [SerializeField] private LayerMask playerLayerMask;
        

        float elapsedTime;

        
        void Update()
        {
            if (IsPlayerInAttackRange())
            {
                Attack();
            }
        }
        
        private bool IsPlayerInAttackRange()
        {
            return Physics.CheckSphere(transform.position, attackRange, playerLayerMask);
        }

        protected override void Attack()
        {
            if (elapsedTime < Time.time)
            {
                PlayAttackAnimation();

                StartCoroutine(IEnableAttacking(attackColliderEnableDelay));
                StartCoroutine(IDisableAttacking(attackColliderEnableDelay + attackDuration));

                elapsedTime = attackCooldown + Time.time;
            }
        }
    }
}
