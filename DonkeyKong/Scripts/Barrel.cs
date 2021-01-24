
using UnityEngine;

namespace DonkeyKong{

    public class Barrel : PoolObject
    {

        [HideInInspector] public BarrelWaypoint currentTarget;
        [SerializeField] float speed;
        [HideInInspector] Rigidbody2D rb;

        Vector2 currentDir = Vector2.zero;

        private void Update(){
            if(Vector2.Distance(transform.position, currentTarget.transform.position) <= 0.1f){
                BarrelWaypoint previousWaypoint = currentTarget;
                if (previousWaypoint.nextWaypoints.Length == 0){
                    gameObject.SetActive(false);
                    return;
                }

                currentTarget = previousWaypoint.nextWaypoints[Random.Range(0, previousWaypoint.nextWaypoints.Length)];
            }else{
                currentDir = (currentTarget.transform.position - transform.position).normalized;
                transform.position += (Vector3)currentDir * speed * Time.deltaTime;
            }

            transform.Rotate(transform.forward, (currentDir.x >= 0.0f ? -360 : 360) * Time.deltaTime);
        }

    }
}