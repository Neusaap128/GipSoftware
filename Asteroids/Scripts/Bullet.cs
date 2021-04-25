
using System.Threading.Tasks;
using UnityEngine;

public class Bullet : PoolObject
{

    [HideInInspector] public float speed = 5;

    private void FixedUpdate(){
        transform.position += transform.up * speed * Time.deltaTime;
    }

    public override void OnRespawn(){
        _ = Disable(2f);
    }

    async Task Disable(float delay){
        await Task.Delay((int)delay*1000);
        gameObject.SetActive(false);
    }

}
