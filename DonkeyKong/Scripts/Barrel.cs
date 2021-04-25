
using UnityEngine;

namespace DonkeyKong{

    public class Barrel : PoolObject
    {

        [HideInInspector] public BarrelWaypoint currentTarget;
        [SerializeField] float speed;
        [HideInInspector] Rigidbody2D rb;

        Vector3 currentDir = Vector3.zero;

        private void Update(){
            if(Vector2.Distance(transform.position, currentTarget.transform.position) <= 0.1f){

                if (currentTarget.nextWaypoints.Length == 0){
                    gameObject.SetActive(false);
                    return;
                }

                BarrelWaypoint previousWaypoint = currentTarget;
                currentTarget = previousWaypoint.nextWaypoints[Random.Range(0, previousWaypoint.nextWaypoints.Length)];
            }else{
                currentDir = (currentTarget.transform.position - transform.position).normalized;
                transform.position += currentDir * speed * Time.deltaTime;
            }

            transform.Rotate(transform.forward, (currentDir.x >= 0.0f ? -360 : 360) * Time.deltaTime);
        }

    }
}