using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters.Common
{
    public abstract class AttackManager : MonoBehaviour
    {
        [Header("Weapon Properties")]
        
        [Tooltip("The animator of the character is needed for playing the attacking animation.")]
        [SerializeField] protected Animator characterAnimator;
        
        [SerializeField] protected string animAttackTriggerParam = "attack";
        
        [Tooltip("This is needed for toggling the weapon collider on and off.")]
        [SerializeField] protected Collider weaponCollider;

        
        [Header("Attack Properties")]
        [Range(0, 10)] [SerializeField] protected float attackColliderEnableDelay;
        [Range(0, 10)] [SerializeField] protected float attackDuration;
        [Range(0, 20)] [SerializeField] protected float attackCooldown;


        protected virtual void Start()
        {
            // Disable the collider at the beginning to prevent attacking all the time.
            StartCoroutine(IDisableAttacking(0));
        }

        protected abstract void Attack();

        protected virtual void PlayAttackAnimation()
        {
            characterAnimator.SetTrigger(animAttackTriggerParam);
        }
        
        protected IEnumerator IEnableAttacking(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            weaponCollider.enabled = true;
        }

        protected IEnumerator IDisableAttacking(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            weaponCollider.enabled = false;
        }
    }
}
