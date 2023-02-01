using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace _Project.Scripts.Gameplay.Characters.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class ChasePlayer : MonoBehaviour
    {
        [Header("Detect Player")] 
        [SerializeField] private Collider _playerCollider;
        
        Transform playerTransform;

        [SerializeField] private LayerMask playerLayerMask;


        [Header("Enemy Properties")] 
        
        [Tooltip("The " + nameof(height) + " is used when raycasting towards the enemy.")]
        [SerializeField] private float height = 5f;
        
        [SerializeField] private float sightRange = 30f;
        
        [Tooltip("The speed of rotating the enemy towards the player")]
        [SerializeField] private float rotateSpeed = 0.4f;
        
        
        [Header("Navigation")]
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private float playerChasingDurationAfterDisappearing;

        float elapsedTime;
        
        
        [Header("Animation")]
        [SerializeField] private Animator animator;

        [Tooltip("This is used to restrict movement while attacking.")]
        [SerializeField] private string animAttackStateName = "attack";
        
        [SerializeField] private string animSpeedFloatParam = "speed";
        
        
        void Start()
        {
            playerTransform = _playerCollider.gameObject.transform;
        }

        void Update()
        {
            UpdateMovementAnimation();
            
            // ABSTRACTION
            HandleFollowStates();
        }
        
        private void UpdateMovementAnimation()
        {
            animator.SetFloat(animSpeedFloatParam, navMeshAgent.velocity.magnitude);
        }

        private void HandleFollowStates()
        {
            if (IsPlayerInSightRange())
            {
                LookAtPlayer();
                
                elapsedTime = Time.time + playerChasingDurationAfterDisappearing;
            }
            // If the game is just started, elapsedTime could be greater than Time.time accidentally.
            if (elapsedTime > Time.time)
            {
                // If the enemy is currently attacking, don't move the enemy. If not, move the enemy
                navMeshAgent.SetDestination(animator.GetCurrentAnimatorStateInfo(0).IsName(animAttackStateName)
                    ? transform.position
                    : playerTransform.position);
            }
        }

        private bool IsPlayerInSightRange()
        {
            if (Physics.Raycast(transform.position + Vector3.up * height, 
                    (playerTransform.position - transform.position).normalized, out var raycastHit, sightRange))
            {
                // I compare the layer that is hit and not the game object because
                // the collider can be in a separate game object rather than the player transform.
                if (HelperMethodsUtil.IsLayerInLayerMask(raycastHit.transform.gameObject.layer, playerLayerMask))
                    return true;
            }

            return false;
        }

        private void LookAtPlayer()
        {
            // I didn't use player transform straightaway because we need a look rotation just on the y-axis.
            Vector3 targetRotation = (playerTransform.position - transform.position).normalized;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetRotation), rotateSpeed);
        }

        #region Methods Used By Other Scripts

        public LayerMask GetPlayerLayerMask()
        {
            return playerLayerMask;
        }

        #endregion
    }
}