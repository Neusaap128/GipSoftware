
using UnityEngine;

namespace Galage{

    public class Enemy : PoolObject{

        [SerializeField] float speed;
        [SerializeField] float fireRate;
        float currentFireRate;

        [SerializeField] Transform firepoint;
        [SerializeField] Transform bullet;

        [SerializeField] int health;

        [HideInInspector] public int x, y;

        [SerializeField] Color[] colors;
        SpriteRenderer sr;

        int rewardScore = 100;

        private void Start(){
            fireRate = Random.Range(fireRate - fireRate / 4, fireRate + fireRate / 4);
            currentFireRate = fireRate;
            sr = GetComponentInChildren<SpriteRenderer>();
            sr.color = colors[0];
        }

        void Update(){

            if(currentFireRate <= 0){
                Fire();
                currentFireRate = fireRate;
            }else{
                currentFireRate -= Time.deltaTime;
            }

        }

        void Fire(){
            PoolObject bullet = PoolManager.RespawnObject(GameManager.BulletPrefab, firepoint.position, firepoint.rotation);
            bullet.GetComponent<Bullet>().speed = 5;
        }

        public void OnTriggerEnter2D(Collider2D collision){
            if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet")){
                collision.gameObject.SetActive(false);
                health--;
                if (health <= 0){
                    Die();
                    return;
                }
                sr.color = colors[3-health];
            }
        }

        public void Die(){
            Player.score += rewardScore;
            Player.UpdateScoreUI();
            GameManager.enemies[x, y] = null;
            gameObject.SetActive(false);
        }

    }
}