
using UnityEngine;

namespace RiskOfRain {

    public class PlayerBullet : PoolObject {

        public float speed;
        public float damage;

        private void FixedUpdate()
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision){

            if(collision.gameObject.layer == LayerMask.NameToLayer("HitAble")){
                collision.GetComponent<HitAble>().TakeDamage(damage);
            }

            gameObject.SetActive(false);
        }


    }

}