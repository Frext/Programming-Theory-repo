using System.Collections;
using UnityEngine;

namespace Living_Things.Common
{
    [RequireComponent(typeof(Collider))]
    public class MeleeAttackHandler : MonoBehaviour
    {
        [SerializeField] private int attackDamage;
        [SerializeField] private AttackManager attackManager;

        [Space] [Tooltip("This is used for the weapon not to injure its owner.")] [SerializeField]
        private GameObject attackerBody;

        // These parameters helps checking if a swung sword enters the enemy body more than once.
        float attackIntervalTime = 0.4f;
        bool canAttack = true;

        private void OnTriggerEnter(Collider other)
        {
            if (canAttack)
            {
                attackManager.DealDamage(attackerBody, other.gameObject, attackDamage);

                canAttack = false;
                
                StartCoroutine(IResetAttackState());
            }
        }

        IEnumerator IResetAttackState()
        {
            yield return new WaitForSeconds(attackIntervalTime);

            canAttack = true;
        }
    }
}