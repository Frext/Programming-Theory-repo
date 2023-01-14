using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Living_Things.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class ChasePlayer : MonoBehaviour
    {
        [Header("Detect Player")] 
        [SerializeField] private Transform playerTransform;

        [SerializeField] private LayerMask _playerLayerMask;
        public LayerMask PlayerLayerMask => _playerLayerMask;

        [Space]
        [SerializeField] private float sightRange = 30f;
        [Tooltip("The speed of rotating the enemy towards the player")]
        [SerializeField] private float rotateSpeed = 0.25f;
        
        [Header("Navigation")]
        [SerializeField] private NavMeshAgent agent;
        
        [Header("Animation")]
        [SerializeField] private Animator animator;
        [Tooltip("This is the name of the attack state in the animator. It's used to restrict movement while attacking.")]
        [SerializeField] private string attackStateName = "attack";

        static readonly int Speed = Animator.StringToHash("speed");

        void Update()
        {
            HandleWalkAnimation();
        }
        
        private void HandleWalkAnimation()
        {
            animator.SetFloat(Speed, agent.velocity.magnitude);
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
                
                // If the enemy is currently attacking, don't move the enemy. If not, move the enemy
                agent.SetDestination(animator.GetCurrentAnimatorStateInfo(0).IsName(attackStateName)
                    ? transform.position
                    : playerTransform.position);
            }
            // If the player disappears, move the enemy to the last sighted point
        }

        private bool IsPlayerInSightRange()
        {
            return Physics.CheckSphere(transform.position, sightRange, PlayerLayerMask);
        }
        
        private void LookAtPlayer()
        {
            // I didn't use player transform straightaway because we need a look rotation just on the y-axis.
            Vector3 targetRotation = (playerTransform.position - transform.position).normalized;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetRotation), rotateSpeed);
        }
    }
}