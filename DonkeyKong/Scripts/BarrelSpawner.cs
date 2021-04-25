
using UnityEngine;

namespace DonkeyKong{

    public class BarrelSpawner : MonoBehaviour
    {

        [SerializeField] PoolObject barrelPrefab;
        [SerializeField] BarrelWaypoint spawnPoint;
        [SerializeField] Vector2 randTimeBtwSpawn;

        Animator anim;

        float currentTimeBtwSpawn;
        float currentTimeBtwStomp;

        private void Start(){
            anim = GetComponentInChildren<Animator>();
            PoolManager.CreatePool(barrelPrefab, 10, transform);
        }

        private void Update(){

            if (!Mario.Alive || Mario.Won)
                return;


            if(currentTimeBtwStomp <= 0){
                anim.SetTrigger("Stomp");
                currentTimeBtwStomp = Random.Range(15, 20);
            }else{
                currentTimeBtwStomp -= Time.deltaTime;
            }

            if (currentTimeBtwSpawn <= 0){
                anim.SetTrigger("ThrowBarrel");
                Invoke("SpawnBarrel", 0.4f);
                currentTimeBtwSpawn = Random.Range(randTimeBtwSpawn.x, randTimeBtwSpawn.y);
            }else{
                currentTimeBtwSpawn -= Time.deltaTime;
            }

        }

        void SpawnBarrel() {

            PoolObject go = PoolManager.RespawnObject(barrelPrefab, spawnPoint.transform.position, Quaternion.identity);
            go.GetComponent<Barrel>().currentTarget = spawnPoint.nextWaypoints[Random.Range(0, spawnPoint.nextWaypoints.Length)];
        
        }

    }
}
