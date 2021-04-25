
using UnityEngine;

namespace RiskOfRain {

    public class FireBall : PlayerBullet
    {

        [SerializeField] GameObject explodeEffect;

        protected override void OnCollision(Collider2D collision){
            Instantiate(explodeEffect, transform.position, Quaternion.identity);
            base.OnCollision(collision);
        }

    }

}