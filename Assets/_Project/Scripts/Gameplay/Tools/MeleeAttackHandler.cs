using System.Collections;
using _Project.Scripts.Gameplay.Characters.Common;
using _Project.Scripts.Gameplay.SFX;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Tools
{
    [RequireComponent(typeof(Collider))]
    public class MeleeAttackHandler : MonoBehaviour
    {
        [Header("Attack Properties")] [SerializeField]
        private int attackDamage;

        [SerializeField] private LayerMask attackLayers;


        [Header("VFX and SFX")]
        [SerializeField] private ParticleSystem bloodParticleEffect;
        [SerializeField] private SFXElement attackSFXElement;

        
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
                    PlayAttackSFX();

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
            if (bloodParticleEffect != null)
            {
                bloodParticleEffect.gameObject.transform.position = contactPoint;
                // Make sure that particle effect and the attacker object are sideways to each other.
                bloodParticleEffect.gameObject.transform.forward = attackerObject.transform.forward * -1;

                bloodParticleEffect.Stop();
                bloodParticleEffect.Play();
            }
        }
        
        private void PlayAttackSFX()
        {
            if (attackSFXElement != null)
            {
                attackSFXElement.PlayOneShotSound();
            }
        }

        IEnumerator IResetAttackState()
        {
            yield return new WaitForSeconds(attackIntervalTime);

            canAttack = true;
        }
    }
}