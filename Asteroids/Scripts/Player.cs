
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroids{

    public class Player : MonoBehaviour{

        [SerializeField] int player;

        float angle = 0;
        [SerializeField] float acceleration;
        [SerializeField] float maxSpeed;
        [SerializeField] float rotateSpeed = 2;
        [Space]
        [SerializeField] PoolObject bullet;
        [SerializeField] float fireRate = 2;
        [SerializeField] Transform firepoint;
        [SerializeField] float bulletSpeed = 10;
        [Space]
        [SerializeField] Vector2 minMaxX;
        [SerializeField] Vector2 minMaxY;
        float currentFireRate;

        [SerializeField] GameObject[] lifeImages;

        int deaths;

        Rigidbody2D rb;

        void Start(){
            currentFireRate = fireRate;
            rb = GetComponent<Rigidbody2D>();
        }

        void Update(){

            float horInput = player == 0 ? Input.GetAxis("HorizontalArrows") : Input.GetAxis("Horizontal");
            float verInput = player == 0 ? Input.GetAxis("VerticalArrows") : Input.GetAxis("Vertical");

            Move(horInput, verInput);

            bool shoot = player == 0 ? Input.GetKeyDown(KeyCode.Space) : Input.GetKeyDown(KeyCode.E);

            if (shoot && currentFireRate <= 0){
                Shoot();
                currentFireRate = fireRate;
            }

            currentFireRate -= Time.deltaTime;

            CheckMapBoundaries();

        }

        void Move(float horInput, float verInput){

            angle -= horInput * rotateSpeed * Time.deltaTime;

            Vector2 velocityAddition = new Vector2(Mathf.Cos((angle + 90) * Mathf.Deg2Rad), Mathf.Sin((angle + 90) * Mathf.Deg2Rad));

            rb.velocity = Vector2.Min(  rb.velocity + velocityAddition * Mathf.Max(verInput, 0) * Time.deltaTime * acceleration,
                                        new Vector2(maxSpeed, maxSpeed));

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        void Shoot(){

            PoolObject poolObject = PoolManager.RespawnObject(bullet, firepoint.position, new Vector3(0, 0, angle));
            poolObject.GetComponent<Bullet>().speed = bulletSpeed;

        }

        void CheckMapBoundaries(){

            if (transform.position.x > minMaxX.y) {
                transform.position = new Vector2(minMaxX.x + 0.2f, transform.position.y);
            }else if(transform.position.x < minMaxX.x) {
                transform.position = new Vector2(minMaxX.y - 0.2f, transform.position.y);
            }else if (transform.position.y > minMaxY.y) {
                transform.position = new Vector2(transform.position.x, minMaxY.x + 0.2f);
            }else if (transform.position.y < minMaxY.x) {
                transform.position = new Vector2(transform.position.x, minMaxY.y - 0.2f);
            }

        }

        public void TakeDamage(){
            lifeImages[deaths].SetActive(false);
            deaths++;
            if (deaths >= 3){
                Die();
                return;
            }
            
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        void Die(){
            GameManager.instance.IncreaseDeaths();
            gameObject.SetActive(false);

        }

    }
}