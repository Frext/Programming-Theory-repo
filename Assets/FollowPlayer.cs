using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class FollowPlayer : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask playerLayerMask;
    
    [Header("Attacking")]
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float attackRange = 7f;
    
    [Space]
    [SerializeField] private float attackInterval = 1f;

    
    private NavMeshAgent agent;
    private Animator animator;

    // Animator parameters
    private static readonly int Speed = Animator.StringToHash("speed");

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleStates();
    }

    private void HandleStates()
    {
        if (IsPlayerInAttackRange())
        {
            agent.SetDestination(playerTransform.position);
            animator.SetFloat(Speed, agent.velocity.magnitude);
        }
        else if (IsPlayerInSightRange())
        {
            LookAtPlayer();
        }
    }

    private bool IsPlayerInSightRange()
    {
        return Physics.CheckSphere(transform.position, sightRange);
    }

    private bool IsPlayerInAttackRange()
    {
        return Physics.CheckSphere(transform.position, attackRange);
    }

    private void LookAtPlayer()
    {
        Vector3 dir = (playerTransform.position - transform.position).normalized;

        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}