
using UnityEngine;
using System.Collections.Generic;

namespace RiskOfRain { 

    public class PlayerStats : MonoBehaviour
    {

        PlayerController playerController;
        [SerializeField] float maxHealth;
        [SerializeField] float currentHealth;
        [Space]
        public PlayerBullet bulletPrefab;
        public Attack primaryAttack;
        [HideInInspector] public float currentTimePrimaryFireRate;
    
        void Start(){
            playerController = GetComponent<PlayerController>();

            currentHealth = maxHealth;

            if (playerController.player == 0){

                List<PoolObject> bullets = PoolManager.CreatePool(bulletPrefab, 40, GameObject.Find("PoolObjects").transform);

                foreach (PoolObject bullet in bullets){
                    bullet.GetComponent<PlayerBullet>().damage = primaryAttack.damage;
                }

            }
        }
    
        public void TakeDamage(float amount){
            
            currentHealth -= amount;
    
            if (currentHealth <= 0)
                Die();
    
        }
    
        void Die(){
    
            Destroy(gameObject);
    
        }
    
    }
}
