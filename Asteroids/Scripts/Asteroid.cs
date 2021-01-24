
using UnityEngine;

namespace Asteroids{

    public class Asteroid : PoolObject{

        [HideInInspector] public float speed;
        [SerializeField] private Asteroid childAsteroid;

        [SerializeField] Vector2 minMaxX;
        [SerializeField] Vector2 minMaxY;

        public void Start(){
            speed = Random.Range(2.0f, 3.0f);
        }

        private void Update(){
            transform.position += transform.up * speed * Time.deltaTime;
            CheckMapBoundaries();
        }

        void Explode(){

            gameObject.SetActive(false);

            if (childAsteroid == null)
                return;

            for (int i = 0; i < Random.Range(2.0f,3.0f); i++)
                 PoolManager.RespawnObject(childAsteroid, transform.position, new Vector3(0, 0, Random.Range(0,360)));
            
        }

        public void OnTriggerEnter2D(Collider2D collision){
            if(collision.gameObject.layer == LayerMask.NameToLayer("Bullet")){
                collision.gameObject.SetActive(false);
                Explode();
            }else if(collision.gameObject.layer == LayerMask.NameToLayer("Player")){
                collision.gameObject.GetComponent<Player>().TakeDamage();
            }
        }


        void CheckMapBoundaries(){

            if (transform.position.x > minMaxX.y) {
                transform.position = new Vector2(minMaxX.x + 0.2f, transform.position.y);
            }else if (transform.position.x < minMaxX.x){
                transform.position = new Vector2(minMaxX.y - 0.2f, transform.position.y);
            }else if (transform.position.y > minMaxY.y){
                transform.position = new Vector2(transform.position.x, minMaxY.x + 0.2f);
            }else if (transform.position.y < minMaxY.x){
                transform.position = new Vector2(transform.position.x, minMaxY.y - 0.2f);
            }

        }

    }
}