using UnityEngine;
using UnityEngine.AI;

namespace Living_Things.Enemy
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class ChasePlayer : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private LayerMask playerLayerMask;
        [SerializeField] private EnemyAttack enemyAttackScript;
    
        [Header("Range")]
        [SerializeField] private float sightRange = 30f;
        [SerializeField] private float attackRange = 7f;

        private NavMeshAgent agent;
        private Animator animator;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            HandleAnimation();
        }

        void FixedUpdate()
        {
            // ABSTRACTION
            HandleStates();
        }

        private void HandleStates()
        {
            if (IsPlayerInAttackRange())
            {
                LookAtPlayer();
                
                // Stop to attack when nearby the player
                ChangeDestination(transform.position);

//                enemyAttackScript.Attack()
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

        private void HandleAnimation()
        {
            animator.SetFloat("speed", agent.velocity.magnitude);
        }

        private void LookAtPlayer()
        {
            /*
            Vector3 dir = (playerTransform.position - transform.position).normalized;

            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            */
            
            transform.LookAt(playerTransform.position, Vector3.up);
        }
    }
}