
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace RiskOfRain
{
    public class EnemySpawner : MonoBehaviour
    {

        private static EnemySpawner instance;

        [SerializeField] Enemy[] allEnemies;
        [SerializeField] float powerScoreMultiplier = 1.2f;

        [SerializeField] float timeBtwWaves;
        float currentTimeBtwWaves;
        [SerializeField] int spawnPortalAfterRounds = 2;
        [Space]
        [SerializeField] float spawnDistFromPlayer;
        [SerializeField] PlayerStats player1;
        [SerializeField] PlayerStats player2;

        [SerializeField] int targetWavePowerScore = 50;

        public static System.Action onWaveEnd;

        List<Enemy> activeEnemies = new List<Enemy>();
        public static List<Enemy> ActiveEnemies => instance.activeEnemies;

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
            if (wave == spawnPortalAfterRounds+1){
                SpawnPortal();
                return;
            }

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
    
            Vector2 spawnpoint = Vector2.zero;

            bool spawnPointInMap = TerrainGenerator.IsPositionInMap(spawnpoint);
            Tile tile = TerrainGenerator.WorldCoordinatesToTile(spawnpoint);
            bool tileInAccessibleRegion = Region.largestRegion.DoesTileLieInRegion(tile);

            int i = 0;
            while (!spawnPointInMap || !tileInAccessibleRegion)
            {
                float distanceBtwPlayers = Vector2.Distance(player1.transform.position, player2.transform.position);
                if (distanceBtwPlayers <= spawnDistFromPlayer * 2 + 5f) {
                    Vector2 centrePointBtwPlayers = Vector2.Lerp(player1.transform.position, player2.transform.position, 0.5f);
                    spawnpoint = Random.insideUnitCircle.normalized * (Random.Range(spawnDistFromPlayer, spawnDistFromPlayer + 5f) + distanceBtwPlayers / 2);
                    spawnpoint += centrePointBtwPlayers;
                } else {
                    Transform randPlayer = Random.value < 0.5f ? player1.transform : player2.transform;
                    spawnpoint = (Vector2)randPlayer.position + Random.insideUnitCircle.normalized * Random.Range(spawnDistFromPlayer, spawnDistFromPlayer + 5f);
                }

                spawnPointInMap = TerrainGenerator.IsPositionInMap(spawnpoint);
                tile = TerrainGenerator.WorldCoordinatesToTile(spawnpoint);
                tileInAccessibleRegion = Region.largestRegion.DoesTileLieInRegion(tile);

                i++;
                if (i >= 50){
                    Debug.Log(spawnPointInMap + ", " + tile + ", " + tileInAccessibleRegion);
                    Debug.Log("Coulnt find");
                    break;
                }
            }

            Enemy go = Instantiate(enemy, spawnpoint, Quaternion.identity);
            go.player1 = player1;
            go.player2 = player2;
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

        GameObject spawnedPortal = null;
        void SpawnPortal(){

            if (spawnedPortal == null) {
                spawnedPortal = TerrainGenerator.SpawnGroundPropGameObject(TerrainGenerator.Sprites.Portal, new Vector3(0.0f, 1.0f), true);
            }else{
                spawnedPortal.SetActive(true);
                spawnedPortal.transform.position = TerrainGenerator.GetRandomAccessibleGroundTilePosition();
            }

            Debug.Log("Spawned Portal at " + spawnedPortal.transform.position);
        }

        public static IEnumerator EndLevel(){
            instance.wave = 0;
            instance.targetWavePowerScore = Mathf.RoundToInt(instance.targetWavePowerScore * 0.75f);
            SceneFader.FadeIn();
            yield return new WaitForSeconds(1.0f);
            instance.spawnedPortal.SetActive(false);
            TerrainGenerator.GenerateTerrain(true);
            SceneFader.FadeOut();
            instance.SpawnWave();
        }

        private void OnDrawGizmos()
        {
            float distanceBtwPlayers = Vector2.Distance(player1.transform.position, player2.transform.position);
            if (distanceBtwPlayers <= spawnDistFromPlayer * 2){

                Vector2 centrePointBtwPlayers = Vector2.Lerp(player1.transform.position, player2.transform.position, 0.5f);
                Gizmos.DrawWireSphere(centrePointBtwPlayers, spawnDistFromPlayer+distanceBtwPlayers/2);

            }else{
                Gizmos.DrawWireSphere(player1.transform.position, spawnDistFromPlayer);
                Gizmos.DrawWireSphere(player2.transform.position, spawnDistFromPlayer);
            }

        }



    }
}