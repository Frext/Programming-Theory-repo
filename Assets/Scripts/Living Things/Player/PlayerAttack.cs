using UnityEngine;
using Living_Things.Common;

namespace Living_Things.Player
{
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

        protected override void Attack()
        {
            if (Input.GetMouseButtonDown(0) && timePassed < Time.time)
            {
                PlayAttackAnimation();

                EnableAttacking();
                Invoke(nameof(DisableAttacking), attackCooldown / 2);
                
                timePassed = attackCooldown + Time.time;
            }
        }

        protected override void PlayAttackAnimation()
        {
            weaponAnimator.SetTrigger("attack_01");
        }
    }
}