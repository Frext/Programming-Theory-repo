using System.Collections;
using _Project.Scripts.Gameplay.Characters.Common;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Tools
{
    [RequireComponent(typeof(Collider))]
    public class MeleeAttackHandler : MonoBehaviour
    {
        [Header("Attack Properties")] 
        [SerializeField] private int attackDamage;

        [SerializeField] private LayerMask attackLayers;
        
        
        [Header("References")]
        [SerializeField] private AttackManager attackManager;
        
        [Tooltip("The collider is needed not to injure the melee owner.")]
        [SerializeField] private Collider _attackerCollider;

        GameObject attackerObject;


        [Header("VFX and SFX")] 
        [SerializeField] private UnityEvent onSuccessfulAttack;

        Vector3 meleeCollisionContactPoint;
        
        
        // These parameters helps checking if a swung melee enters the enemy body more than once.
        readonly float attackIntervalTime = 0.4f;
        bool canAttack = true;

        
        void Awake()
        {
            attackerObject = _attackerCollider.gameObject;
        }

        void OnTriggerEnter(Collider other)
        {
            if (canAttack && HelperMethodsUtil.IsLayerInLayerMask(other.gameObject.layer, attackLayers))
            {
                if (DealDamage(attackerObject, other.gameObject, attackDamage))
                {
                    UpdateCollisionContactPoint(other.ClosestPoint(transform.position));
                    
                    onSuccessfulAttack.Invoke();

                    
                    canAttack = false;
                }

                StartCoroutine(IResetAttackState());
            }
        }

        static bool DealDamage(GameObject attackerObject, GameObject objectToDealDamage, int damageAmount)
        {
            if (objectToDealDamage != attackerObject && objectToDealDamage != null)
            {
                HealthManager healthManager = objectToDealDamage.GetComponent<HealthManager>();
                
                if (healthManager != null)
                {
                    healthManager.TakeDamage(damageAmount);

                    return true;
                }
            }

            return false;
        }

        private void UpdateCollisionContactPoint(Vector3 newPoint)
        {
            meleeCollisionContactPoint = newPoint;
        }
        
        IEnumerator IResetAttackState()
        {
            yield return new WaitForSeconds(attackIntervalTime);

            canAttack = true;
        }

        #region Methods Used By Other Scripts

        public void PlayBloodParticleEffectAtCollisionPoint(ParticleSystem bloodParticleEffect)
        {
            if (bloodParticleEffect != null)
            {
                bloodParticleEffect.gameObject.transform.position = meleeCollisionContactPoint;
                
                // Make sure that particle effect and the attacker object are sideways to each other.
                bloodParticleEffect.gameObject.transform.forward = attackerObject.transform.forward * -1;

                bloodParticleEffect.Stop();
                bloodParticleEffect.Play();
            }
        }

        #endregion
    }
}