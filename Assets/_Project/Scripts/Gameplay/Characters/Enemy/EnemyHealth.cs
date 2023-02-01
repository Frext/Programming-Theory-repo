using _Project.Scripts.Gameplay.Characters.Common;

namespace _Project.Scripts.Gameplay.Characters.Enemy
{
    public class EnemyHealth : HealthManager
    {
        #region Methods Used By Other Scripts
        
        // This is used when an enemy returns to the pool, so it must be public.
        public void ResetHealth()
        {
            Health = maxHealth;
        }
        
        #endregion
    }
}
