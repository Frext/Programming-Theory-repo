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
            Attack();
        }
        
        // POLYMORPHISM
        protected override void Attack()
        {
            if (Input.GetButton("Fire1") && elapsedTime < Time.time)
            {
                PlayAttackAnimation();

                StartCoroutine(IEnableAttacking(attackColliderEnableDelay));
                StartCoroutine(IDisableAttacking((attackColliderEnableDelay + attackCooldown) / 2));
                
                elapsedTime = attackColliderEnableDelay + attackCooldown + Time.time;
            }
        }

        protected override void PlayAttackAnimation()
        {
            characterAnimator.SetTrigger(characterAttackParamName);
        }
    }
}