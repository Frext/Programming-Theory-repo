using System;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Gameplay.Characters.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class ChasePlayer : MonoBehaviour
    {
        [Header("Detect Player")] 
        [SerializeField] private Collider _playerCollider;
        Transform playerTransform;

        [SerializeField] private LayerMask _playerLayerMask;
        public LayerMask PlayerLayerMask => _playerLayerMask;
        
        [Tooltip("This is used to avoid enemies seeing the player through buildings or other obstacles.")]
        [SerializeField] private LayerMask buildingLayerMask;
        
        [Space]
        [SerializeField] private float sightRange = 30f;
        [Tooltip("The speed of rotating the enemy towards the player")]
        [SerializeField] private float rotateSpeed = 0.25f;
        
        
        [Header("Navigation")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float playerChasingDurationAfterDisappearing;

        float elapsedTime = 0f;
        
        
        [Header("Animation")]
        [SerializeField] private Animator animator;
        [Tooltip("This is the name of the attack state in the animator. It's used to restrict movement while attacking.")]
        [SerializeField] private string attackStateName = "attack";

        static readonly int Speed = Animator.StringToHash("speed");

        void Start()
        {
            playerTransform = _playerCollider.gameObject.transform;
        }

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
                
                elapsedTime = Time.time + playerChasingDurationAfterDisappearing;
            }
            if (elapsedTime > Time.time)
            {
                // If the enemy is currently attacking, don't move the enemy. If not, move the enemy
                agent.SetDestination(animator.GetCurrentAnimatorStateInfo(0).IsName(attackStateName)
                    ? transform.position
                    : playerTransform.position);
            }
        }

        private bool IsPlayerInSightRange()
        {
            RaycastHit raycastHit;
            

            if (Physics.Raycast(transform.position + Vector3.up * 5, (playerTransform.position - transform.position).normalized * 100, out raycastHit, sightRange))
            {
                LayerMask layerHit = raycastHit.transform.gameObject.layer;
                
                // I compare the layer and not the game object because the collider can be in a separate game object rather than the player transform.
                if (IsInLayer(PlayerLayerMask,layerHit) ||  !IsInLayer(buildingLayerMask, layerHit))
                    return true;
            }

            return false;
        }
        
        private bool IsInLayer(LayerMask layerMask,int layer)
        {
            // Returns true if the layer that is converted into a layer mask and the attack layer mask have a common bit which is 1.
            return (layerMask & (1 << layer)) != 0;
        }
        
        private void LookAtPlayer()
        {
            // I didn't use player transform straightaway because we need a look rotation just on the y-axis.
            Vector3 targetRotation = (playerTransform.position - transform.position).normalized;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetRotation), rotateSpeed);
        }
    }
}