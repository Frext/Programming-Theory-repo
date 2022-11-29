using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Living_Things.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class ChasePlayer : MonoBehaviour
    {
        [Header("Player")] 
        [SerializeField] private Transform playerTransform;
        [SerializeField] private LayerMask playerLayerMask;
        
        [Space]
        [SerializeField] private float sightRange = 30f;
        
        [Space]
        [SerializeField] private NavMeshAgent agent;
        
        [Header("Animation")]
        [SerializeField] private Animator animator;
        [Tooltip("This is the name of the attack state in the animator. It's used to restrict movement while attacking.")]
        [SerializeField] private string attackStateName;

        static readonly int Speed = Animator.StringToHash("speed");

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
                
                // If the enemy is attacking, don't move the enemy. If not, move the enemy
                ChangeDestination(animator.GetCurrentAnimatorStateInfo(0).IsName(attackStateName)
                    ? transform.position
                    : playerTransform.position);
            }
            // If the player disappears, move the enemy to the last sighted point
        }

        private bool IsPlayerInSightRange()
        {
            return Physics.CheckSphere(transform.position, sightRange, playerLayerMask);
        }

        private void ChangeDestination(Vector3 destination)
        {
            while (true)
            {
                if (agent.SetDestination(destination))
                    break;
            }
        }

        private void HandleWalkAnimation()
        {
            animator.SetFloat(Speed, agent.velocity.magnitude);
        }

        private void LookAtPlayer()
        {
            /*
            Vector3 dir = (playerTransform.position - transform.position).normalized;

            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            */
            
            // I didn't use player transform straightaway because we need a look rotation just on the y-axis.
            Vector3 targetPosition = playerTransform.position;
            targetPosition.y = 0;
            
            transform.LookAt(new Vector3(targetPosition.x, 0, targetPosition.z));
        }
    }
}