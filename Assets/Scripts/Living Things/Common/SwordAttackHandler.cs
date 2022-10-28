using System;
using Living_Things.Enemy;
using UnityEngine;

namespace Living_Things.Common
{
    public class SwordAttackHandler : MonoBehaviour
    {
        [SerializeField] private int attackDamage;
        [SerializeField] private AttackManager attackManager;

        [Space] 
        [Tooltip("This is used for the weapon not to injure its owner.")]
        [SerializeField] private GameObject attackerBody;
        
        private void OnTriggerEnter(Collider other)
        {
            attackManager.DealDamage(attackerBody,other.gameObject, attackDamage);
        }
    }
}
