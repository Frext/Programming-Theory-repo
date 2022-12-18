using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Living_Things.Common
{
    [RequireComponent(typeof(Collider))]
    public class MeleeAttackHandler : MonoBehaviour
    {
        [SerializeField] private int attackDamage;
        [SerializeField] private LayerMask attackLayers;
        
        [SerializeField] private AttackManager attackManager;

        [Space]
        [Tooltip("This is used for the weapon not to injure its owner. The game object with the health script must be given.")] 
        [SerializeField] private GameObject attackerColliderBody;
        
        
        // These parameters helps checking if a swung sword enters the enemy body more than once.
        readonly float attackIntervalTime = 0.4f;
        bool canAttack = true;

        private void OnTriggerEnter(Collider other)
        {
            if (canAttack && IsInAttackLayers(other.gameObject.layer))
            {
                bool didDealDamage = attackManager.DealDamage(attackerColliderBody, other.gameObject, attackDamage);

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