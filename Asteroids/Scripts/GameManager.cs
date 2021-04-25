
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroids{

    public class GameManager : MonoBehaviour{

        public static GameManager instance;
        [SerializeField] PoolObject asteroid1;
        [SerializeField] PoolObject asteroid2;
        [SerializeField] PoolObject asteroid3;

        [SerializeField] float timeBtwAsteroids;
        [SerializeField] Vector2 asteroidSpawnX;
        [SerializeField] Vector2 asteroidSpawnY;
        float currentTimeBtwAsteroids;

        [SerializeField] GameObject gameOverText;

        static int deaths;


        void Start(){
            instance = this;

            PoolManager.CreatePool(asteroid1, 5, transform);
            PoolManager.CreatePool(asteroid2, 10, transform);
            PoolManager.CreatePool(asteroid3, 20, transform);

            
            SpawnAsteroid();
           

        }

        private void Update() {
            
            /*
            if(currentTimeBtwAsteroids <= 0.0f){
                SpawnAsteroid();
                currentTimeBtwAsteroids = timeBtwAsteroids;
            }else{
                currentTimeBtwAsteroids -= Time.deltaTime;
            }
            */
        }

        void SpawnAsteroid(){

            bool useX = Random.value >= 0.5f ? false : true;
            bool left = Random.value >= 0.5f ? false : true;
            float x = useX ? (left ? asteroidSpawnX.x : asteroidSpawnX.y) : (Random.Range(asteroidSpawnX.x, asteroidSpawnX.y));

            bool top = Random.value >= 0.5f ? false : true;
            float y = !useX ? (top ? asteroidSpawnY.y : asteroidSpawnY.x) : (Random.Range(asteroidSpawnY.x, asteroidSpawnY.y));

            Vector3 pos = new Vector3(x, y);

            Vector3 dir = (-pos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x);
            dir = new Vector3(0, 0, angle*Mathf.Rad2Deg);
            PoolManager.RespawnObject(asteroid1, pos, dir);

        }

        public void IncreaseDeaths(){
            deaths++;
            if(deaths >= 2)
                GameOver();
            
        }

        void GameOver(){
            gameOverText.SetActive(true);
            Invoke("ReloadScene", 2f);
        }

        void ReloadScene(){ 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


    }
}