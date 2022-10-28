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
            if (Input.GetMouseButtonDown(0) && timePassed < Time.time)
            {
                Attack();

                timePassed = attackCooldown + Time.time;
            }
        }

        protected override void Attack()
        {
            PlayAttackAnimation();

            EnableAttacking();

            Invoke(nameof(DisableAttacking), 0.5f);
        }

        protected override void PlayAttackAnimation()
        {
            animator.SetTrigger("attack_01");
        }

        private void EnableAttacking()
        {
            weaponCollider.enabled = true;
        }

        private void DisableAttacking()
        {
            weaponCollider.enabled = false;
        }
    }
}