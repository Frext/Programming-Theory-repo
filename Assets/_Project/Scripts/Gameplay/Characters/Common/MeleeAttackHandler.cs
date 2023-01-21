using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters.Common
{
    [RequireComponent(typeof(Collider))]
    public class MeleeAttackHandler : MonoBehaviour
    {
        [SerializeField] private int attackDamage;
        [SerializeField] private LayerMask attackLayers;
        
        [SerializeField] private AttackManager attackManager;

        [Space]
        [Tooltip("The game object the health manager is in, the one with the collider, is needed not to injure the melee owner.")] 
        [SerializeField] private HealthManager healthManagerScript;


        GameObject attackerObject;
        
        // These parameters helps checking if a swung melee enters the enemy body more than once.
        readonly float attackIntervalTime = 0.4f;
        bool canAttack = true;

        void Awake()
        {
            attackerObject = healthManagerScript.gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (canAttack && IsInAttackLayers(other.gameObject.layer))
            {
                bool didDealDamage = attackManager.DealDamage(attackerObject, other.gameObject, attackDamage);

                if (didDealDamage)
                    canAttack = false;

                StartCoroutine(IResetAttackState());
            }
        }

        private bool IsInAttackLayers(int layer)
        {
            // Returns true if the layer that is converted into a layer mask and the attack layer mask have a common bit which is 1.
            return (attackLayers & (1 << layer)) != 0;
        }

        IEnumerator IResetAttackState()
        {
            yield return new WaitForSeconds(attackIntervalTime);

            canAttack = true;
        }
    }
}