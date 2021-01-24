
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace RiskOfRain
{
    public class EnemySpawner : MonoBehaviour
    {

        private static EnemySpawner instance;

        [SerializeField] Enemy[] allEnemies;
        [SerializeField] float powerScoreMultiplier = 1.5f;

        [SerializeField] float timeBtwWaves;
        float currentTimeBtwWaves;
        [Space]
        [SerializeField] float spawnDistFromPlayer;
        [SerializeField] Transform player1;
        [SerializeField] Transform player2;

        int targetWavePowerScore = 100;

        public static System.Action onWaveEnd;

        List<Enemy> activeEnemies = new List<Enemy>();

        bool spawnNextWave = false;
        float timeToNextWave;

        int wave = 0;

        private void Awake(){
            if (instance == null)
                instance = this;
        }

        void Start(){
            SpawnWave();
        }

        private void Update(){

            if (spawnNextWave) { 
                if (timeToNextWave < 0){
                    SpawnWave();
                }else{
                    timeToNextWave -= Time.deltaTime;
                }
            }
        }

        public static void RemoveEnemy(Enemy enemy){
            instance.activeEnemies.Remove(enemy);
            instance.CheckAliveEnemyCount();
        }

        void CheckAliveEnemyCount(){

            int count = activeEnemies.Count;

            if (count <= 0)
                EndWave();

        }

        void SpawnWave(){

            spawnNextWave = false;

            wave++;

            Debug.Log("Wave: " + wave);
            if (wave == 10)
                EndLevel();

            int totalWavePower = 0;
            while (totalWavePower < targetWavePowerScore){

                Enemy randEnemy = allEnemies[Random.Range(0, allEnemies.Length)];

                if ((randEnemy.biomes & TerrainGenerator.Biome) != 0){
                    SpawnEnemy(randEnemy);
                    totalWavePower += randEnemy.powerScore;
                }
            }

        }
    
        void SpawnEnemy(Enemy enemy){
    
            Vector2 spawnPoint = Vector2.zero;
    
            float distanceBtwPlayers = Vector2.Distance(player1.position, player2.position);
            if (distanceBtwPlayers <= spawnDistFromPlayer * 2){
                Vector2 centrePointBtwPlayers = Vector2.Lerp(player1.position, player2.position, 0.5f);
                spawnPoint = Random.insideUnitCircle.normalized * (spawnDistFromPlayer + distanceBtwPlayers / 2);
                spawnPoint += centrePointBtwPlayers;
    
            }else{
                Transform randPlayer = Random.value < 0.5f ? player1 : player2;
                spawnPoint = (Vector2)randPlayer.position + Random.insideUnitCircle.normalized * spawnDistFromPlayer;
            }

            Enemy go = Instantiate(enemy, spawnPoint, Quaternion.identity);
            activeEnemies.Add(go);

        }

        void EndWave(){

            targetWavePowerScore = Mathf.RoundToInt(targetWavePowerScore * powerScoreMultiplier);
            timeToNextWave = timeBtwWaves;
            spawnNextWave = true;

            if (onWaveEnd != null)
                onWaveEnd.Invoke();

            Debug.Log("Wave End");

        }

        void EndLevel(){
            wave = 0;
            Debug.Log("Regenerating Terrain");
            TerrainGenerator.GenerateTerrain(true);
        }

        private void OnDrawGizmos()
        {
            float distanceBtwPlayers = Vector2.Distance(player1.position, player2.position);
            if (distanceBtwPlayers <= spawnDistFromPlayer * 2){

                Vector2 centrePointBtwPlayers = Vector2.Lerp(player1.position, player2.position, 0.5f);
                Gizmos.DrawWireSphere(centrePointBtwPlayers, spawnDistFromPlayer+distanceBtwPlayers/2);

            }else{
                Gizmos.DrawWireSphere(player1.position, spawnDistFromPlayer);
                Gizmos.DrawWireSphere(player2.position, spawnDistFromPlayer);
            }
        }

    }
}