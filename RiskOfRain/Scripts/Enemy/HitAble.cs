
using UnityEngine;

namespace RiskOfRain
{
    public class HitAble : MonoBehaviour
    {
    
        [SerializeField] protected float maxHealth;
        protected float currentHealth;
    
        void Start(){
            currentHealth = maxHealth;
        }
    
        public virtual void TakeDamage(float amount){
    
            Debug.Log(gameObject.name + " received " + amount + " damage");
    
            currentHealth -= amount;
    
            if (currentHealth <= 0)
                Die();
        }
    
        protected virtual void Die(){
            Destroy(gameObject);
        }
    
    }
}