using UnityEngine;

namespace Living_Things
{
    public class PlayerAttack : AttackManager
    {
        float timePassed;


        void Start()
        {
            DisableAttacking();
        }

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
            animator.SetTrigger("attack_01");
        }
    }
}