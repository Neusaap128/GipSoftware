
using UnityEngine;

namespace RiskOfRain {

    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
    public class PlayerBullet : PoolObject {

        public float speed;
        [HideInInspector] public float damage;

        void FixedUpdate(){
            transform.position += transform.up * speed * Time.deltaTime;
        }

        void OnTriggerEnter2D(Collider2D collision){
            OnCollision(collision);
        }

        protected virtual void OnCollision(Collider2D collision){

            if (collision.gameObject.layer == LayerMask.NameToLayer("HitAble")){
                collision.GetComponent<HitAble>().TakeDamage(damage);
            }

            gameObject.SetActive(false);
        }

        public override void OnRespawn(){
            this.SetActive(false, 5f);
        }

    }


}