using _Project.Scripts.Gameplay.Characters.Common;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters.Player
{
    // INHERITANCE
    public class PlayerAttack : AttackManager
    {
        float elapsedTime;

        void Update()
        {
            GetAttackInput();
        }

        private void GetAttackInput()
        {
            if (Input.GetButton("Fire1") && elapsedTime < Time.time)
            {
                Attack();
                
                elapsedTime = attackCooldown + Time.time;
            }
        }

        // POLYMORPHISM
        protected override void Attack()
        {
            PlayAttackAnimation();

            StartCoroutine(IEnableAttacking(attackColliderEnableDelay));
            StartCoroutine(IDisableAttacking(attackColliderEnableDelay + attackDuration));
        }
    }
}