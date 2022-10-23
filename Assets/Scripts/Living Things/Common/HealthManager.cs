using UnityEngine;

namespace Living_Things
{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField] private int initialHealth;

        // ENCAPSULATION
        private int _health;

        private int Health
        {
            get => _health;
            set
            {
                if (value >= 0)
                {
                    _health = value;
                }
            }
        }
    
        void Start()
        {
            Health = initialHealth;
        }

        public void GetDamage(int damageAmount)
        {
            Health -= damageAmount;
        }
    }
}