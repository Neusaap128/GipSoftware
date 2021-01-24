
using UnityEngine;

public class Bullet : PoolObject
{

    [HideInInspector] public float speed = 5;

    void Disable(){
        gameObject.SetActive(false);
    }

    private void FixedUpdate(){
        transform.position += transform.up * speed * Time.deltaTime;
    }

    public override void OnRespawn(){
        Invoke("Disable", 2);
    }

}
