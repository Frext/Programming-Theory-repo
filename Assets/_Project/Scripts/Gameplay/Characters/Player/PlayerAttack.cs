using _Project.Scripts.Gameplay.Characters.Common;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters.Player
{
    // INHERITANCE
    public class PlayerAttack : AttackManager
    {
        float timePassed;

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
            if (Input.GetButton("Fire1") && timePassed < Time.time)
            {
                PlayAttackAnimation();

                EnableAttacking();
                Invoke(nameof(DisableAttacking), attackCooldown / 2);
                
                timePassed = attackCooldown + Time.time;
            }
        }

        protected override void PlayAttackAnimation()
        {
            characterAnimator.SetTrigger(characterAttackParamName);
        }
    }
}