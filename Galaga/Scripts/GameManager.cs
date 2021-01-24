
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Galage {

    public class GameManager : MonoBehaviour{

        public static GameManager instance;

        const int enemyGridWidth = 15;
        const int enemyGridHeight = 5;
        public static Enemy[,] enemies = new Enemy[enemyGridWidth, enemyGridHeight];

        [SerializeField] Enemy enemyPrefab;
        public static PoolObject BulletPrefab;
        [SerializeField] PoolObject bulletPrefab;

        [SerializeField] float timeBtwWaves = 10;
        float currentTimeBtwWaves = 1;

        static int totalDeaths;

        [SerializeField] GameObject gameOverText;

        void Start(){
            instance = this;
            PoolManager.CreatePool(enemyPrefab, 45);
            PoolManager.CreatePool(bulletPrefab, 40);
            BulletPrefab = bulletPrefab;
        }

        void Update(){

            if(currentTimeBtwWaves <= 0){
                SpawnWave();
                currentTimeBtwWaves = timeBtwWaves;
                timeBtwWaves = Mathf.Max(1.5f, timeBtwWaves - 0.25f);
            }else{
                currentTimeBtwWaves -= Time.deltaTime;
            }
        }

        void SpawnWave(){

            int xStart = Random.Range(0, enemyGridWidth);
            int yStart = Random.Range(0, enemyGridHeight);

            int xEnd = Random.Range(xStart+1, enemyGridWidth);
            int yEnd = Random.Range(yStart+1, enemyGridHeight);

            for (int x = xStart; x < xEnd; x++){
                for(int y = yStart; y < yEnd; y++){
                    if (x >= enemyGridWidth || y >= enemyGridHeight)
                        continue;

                    if(enemies[x,y] == null){
                        SpawnEnemy(x, y);
                    }
                }
            }

        }

        void SpawnEnemy(int x, int y){
            Enemy enemy = PoolManager.RespawnObject(enemyPrefab, new Vector2(x - Mathf.CeilToInt(enemyGridWidth / 2), y), Quaternion.identity).GetComponent<Enemy>();
            enemy.x = x;
            enemy.y = y;
            enemies[x , y] = enemy;
        }

        public void IncreaseTotalDeaths(){
            totalDeaths++;
            if (totalDeaths >= 2)
                GameOver();
        }

        void GameOver(){
            gameOverText.SetActive(true);
            Invoke("ReloadScene",3f);
        }

        void ReloadScene(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}