
using UnityEngine;
using UnityEngine.UI;

namespace Galage{

    public class Player : MonoBehaviour{

        [SerializeField] float speed;
        [SerializeField] PoolObject bullet;
        [SerializeField] Transform firepoint;
        [SerializeField] float fireRate;
        float currentFireRate;

        [SerializeField] int player = 0;

        public static int score;
        public static Text ScoreText;
        [SerializeField] Text scoreText;

        int health = 3;

        void Start(){
            ScoreText = scoreText;
            if(player==0)
                PoolManager.CreatePool(bullet, 20);
        }

        void Update(){

            float inputHor = player == 0 ? Input.GetAxis("HorizontalArrows") : Input.GetAxis("Horizontal");

            transform.position += transform.right * inputHor * speed * Time.deltaTime;

            bool shoot = player == 0 ? Input.GetKeyDown(KeyCode.Space) : Input.GetKeyDown(KeyCode.E);

            if (shoot && currentFireRate <= 0.0f) {
                Fire();
                currentFireRate = fireRate;
            }
            currentFireRate -= Time.deltaTime;
        }

        void Fire(){
            PoolObject bullet = PoolManager.RespawnObject(this.bullet, firepoint.position, firepoint.rotation);
            bullet.GetComponent<Bullet>().speed = 10;
        }

        public static void UpdateScoreUI(){
            ScoreText.text = "Total Score: " + score;
        }

        public void OnTriggerEnter2D(Collider2D collision){
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBullet")){
                collision.gameObject.SetActive(false);
                health--;
                if (health <= 0)
                    Die();
                
            }
        }

        void Die(){
            GameManager.instance.IncreaseTotalDeaths();
            gameObject.SetActive(false);
        }

    }
}