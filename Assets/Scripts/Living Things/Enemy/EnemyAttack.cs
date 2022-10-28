using System;
using UnityEngine;

namespace Living_Things.Enemy
{
    public class EnemyAttack : AttackManager
    {
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            throw new NotImplementedException();
        }

        protected override void Attack()
        {
            PlayAttackAnimation();
        }

        protected override void PlayAttackAnimation()
        {
            animator.SetTrigger("attack_01");
        }
    }
}
