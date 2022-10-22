using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class FollowPlayer : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private LayerMask playerLayerMask;
    
        [Header("Range")]
        [SerializeField] private float sightRange = 30f;
        [SerializeField] private float attackRange = 7f;

        private NavMeshAgent agent;
        private Animator animator;

        private enum EnemyAnimations {
            Walk,
            Attack
        }

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            HandleAnimation(EnemyAnimations.Walk);
            HandleStates();
        }

        private void HandleStates()
        {
            if (IsPlayerInAttackRange())
            {
                LookAtPlayer();
                
                ChangeDestination(transform.position);
                HandleAnimation(EnemyAnimations.Attack);
                // TODO: AttackScript.Attack()
            }
            else if (IsPlayerInSightRange())
            {
                LookAtPlayer();
                
                ChangeDestination(playerTransform.position);
            }
            else
            {
                ChangeDestination(transform.position);
            }
        }

        private bool IsPlayerInSightRange()
        {
            return Physics.CheckSphere(transform.position, sightRange, playerLayerMask);
        }

        private bool IsPlayerInAttackRange()
        {
            return Physics.CheckSphere(transform.position, attackRange, playerLayerMask);
        }

        private void ChangeDestination(Vector3 destination)
        {
            agent.SetDestination(destination);
        }

        private void HandleAnimation(EnemyAnimations animation)
        {
            if (animation == EnemyAnimations.Walk) {
                animator.SetFloat("speed", agent.velocity.magnitude);
            }
            if(animation == EnemyAnimations.Attack) {
                animator.SetTrigger("attack");
            }
        }

        private void LookAtPlayer()
        {
            Vector3 dir = (playerTransform.position - transform.position).normalized;

            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
    }
}