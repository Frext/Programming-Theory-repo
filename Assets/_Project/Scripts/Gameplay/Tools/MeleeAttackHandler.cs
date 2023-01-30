using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Project.Scripts.Gameplay.Characters.Common
{
    [RequireComponent(typeof(Collider))]
    public class MeleeAttackHandler : MonoBehaviour
    {
        [Header("Attack Properties")]
        [SerializeField] private int attackDamage;
        [SerializeField] private LayerMask attackLayers;
        
        [Space]
        [SerializeField] private List<ParticleSystem> particleEffects;
        
        
        [Header("Script References")]
        [SerializeField] private AttackManager attackManager;
        
        [Space]
        [Tooltip("The game object the health manager is in, the one with the collider, is needed not to injure the melee owner.")] 
        [SerializeField] private HealthManager _healthManagerScript;


        GameObject attackerObject;
        
        // These parameters helps checking if a swung melee enters the enemy body more than once.
        readonly float attackIntervalTime = 0.4f;
        bool canAttack = true;

        void Awake()
        {
            attackerObject = _healthManagerScript.gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (canAttack && IsInAttackLayers(other.gameObject.layer))
            {
                bool didDealDamage = attackManager.DealDamage(attackerObject, other.gameObject, attackDamage);

                if (didDealDamage)
                {
                    PlayParticleEffects(other.ClosestPoint(transform.position));
                    
                    canAttack = false;
                }

                StartCoroutine(IResetAttackState());
            }
        }
        
        private bool IsInAttackLayers(int layer)
        {
            // Returns true if the layer that is converted into a layer mask and the attack layer mask have a common bit which is 1.
            return (attackLayers & (1 << layer)) != 0;
        }
        
        private void PlayParticleEffects(Vector3 contactPoint)
        {
            foreach (ParticleSystem particleEffect in particleEffects)
            {
                if (particleEffect != null)
                {
                    particleEffect.gameObject.transform.position = contactPoint;
                    // Make sure that particle effect and the attacker object are sideways to each other.
                    particleEffect.gameObject.transform.forward = attackerObject.transform.forward * -1;

                    particleEffect.Stop();
                    particleEffect.Play();
                }
            }
        }

        IEnumerator IResetAttackState()
        {
            yield return new WaitForSeconds(attackIntervalTime);

            canAttack = true;
        }
    }
}