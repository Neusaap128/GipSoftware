using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiskOfRain {

    [SelectionBase]
    public class ExplosiveBarrel : HitAble {

        [SerializeField] float explosionDamage;
        [SerializeField] float explosionRadius;

        [SerializeField] PoolObject explosionEffect;

        private void Start(){
            PoolManager.CreatePool(explosionEffect, 1, GameObject.Find("PoolObjects").transform);
        }

        protected override void Die(){

            Collider2D[] collision = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

            foreach (Collider2D collider in collision){

                if (collider.gameObject.TryGetComponent(out HitAble hitable)){
                    if (hitable != this && hitable.gameObject.activeSelf)
                        hitable.TakeDamage(explosionDamage);
                }
                
            }

            PoolManager.RespawnObject(explosionEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            
        }

        private void OnDrawGizmosSelected(){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

    }
}