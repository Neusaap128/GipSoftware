
using UnityEngine;

namespace RiskOfRain
{
    [SelectionBase]
    public class HitAble : MonoBehaviour
    {
    
        [Header("Hit-Able")]
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float currentHealth;
        [SerializeField] ItemDrop[] possibleDrops;

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
            
        }
    
    }

}