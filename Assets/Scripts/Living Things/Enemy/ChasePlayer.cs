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

        [Header("Range")] 
        [SerializeField] private float sightRange = 30f;

        private NavMeshAgent agent;
        private Animator animator;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            HandleWalkAnimation();
        }

        void FixedUpdate()
        {
            // ABSTRACTION
            HandleStates();
        }

        private void HandleStates()
        {
            if (IsPlayerInSightRange())
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

        private void ChangeDestination(Vector3 destination)
        {
            agent.SetDestination(destination);
        }

        private void HandleWalkAnimation()
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