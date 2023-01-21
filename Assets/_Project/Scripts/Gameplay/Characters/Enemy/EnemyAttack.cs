using _Project.Scripts.Gameplay.Characters.Common;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters.Enemy
{
    public class EnemyAttack : AttackManager
    {
        [Range(0, 50)]
        [SerializeField] private float attackRange;

        [Header("Detect Player")]
        [Tooltip("This script is used for getting the layer mask of player.")]
        [SerializeField] private ChasePlayer chasePlayerScript;
        
        
        float elapsedTime;

        void Update()
        {
            if (IsPlayerInAttackRange())
            {
                Attack();
            }
        }

        protected override void Attack()
        {
            if (elapsedTime < Time.fixedTime)
            {
                PlayAttackAnimation();

                StartCoroutine(IEnableAttacking(attackColliderEnableDelay));
                StartCoroutine(IDisableAttacking((attackColliderEnableDelay + attackCooldown) / 2));

                elapsedTime = attackColliderEnableDelay + attackCooldown + Time.fixedTime;
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
