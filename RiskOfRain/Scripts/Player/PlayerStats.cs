
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RiskOfRain { 

    public class PlayerStats : HitAble
    {

        PlayerController playerController;
        [SerializeField] Slider healthBar;
        [Space]
        public Attack primaryAttack;
        [HideInInspector] public float currentTimePrimaryFireRate;
        public Attack secondaryAttack;
        [HideInInspector] public float currentTimeSecondaryFireRate;

        [SerializeField] List<Item> inventory = new List<Item>();
        public ReadOnlyCollection<Item> Inventory => inventory.AsReadOnly();
        public int totalDamageAddition { get; private set; }
        public float totalFireRateAddition { get; private set; }

        void Start(){

            currentHealth = maxHealth;
            UpdateHealthBar();

            playerController = GetComponent<PlayerController>();

            GameObject parent = GameObject.Find("PoolObjects");

            //Create a pool of bullets
            PoolManager.CreatePool(primaryAttack.bulletPrefab, 20, parent.transform);
            PoolManager.CreatePool(secondaryAttack.bulletPrefab, 20, parent.transform);

        }

        public override void TakeDamage(float amount){
            base.TakeDamage(amount);
            healthBar.value = currentHealth;
        }

        protected override void Die(){
            Destroy(gameObject);
        }

        void UpdateHealthBar(){
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    
        public void AddItemToInventory(Item item){
            totalDamageAddition += item.damageAddition;
            totalFireRateAddition += item.fireRateAddition;

            playerController.speed += (10 - playerController.speed) * item.speedPercentIncrease;
            playerController.jumpForce += (12 - playerController.jumpForce) * item.jumpHeightIncrease;

            maxHealth += item.maxHealthAddition;

            UpdateHealthBar();

            inventory.Add(item);
        }

    }
}
