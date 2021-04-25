
using UnityEngine;

public class ExplosionEffect : PoolObject
{

    private void Start(){
        OnRespawn();
    }

    public override void OnRespawn(){
        Destroy(gameObject, 2f);
    }

}
