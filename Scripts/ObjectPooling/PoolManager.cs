
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public static class PoolManager
{

    static Dictionary<PoolObject, List<PoolObject>> pools = new Dictionary<PoolObject, List<PoolObject>>();

    /// <summary>
    /// Create a pool of objects that can be re-instantiated.
    /// </summary>
    /// <param name="poolObject"> Reference of object </param>
    /// <param name="size"> Amount of elements that need to be in the pool </param>
    /// <param name="parent"> optional parent for the pool </param>
    /// <returns></returns>
    public static List<PoolObject> CreatePool(PoolObject poolObject, int size, Transform parent = null){

        PoolObject[] objects = new PoolObject[size];

        for (int i = 0; i < size; i++){
            objects[i] = GameObject.Instantiate(poolObject, parent);
            objects[i].gameObject.SetActive(false);
        }

        SceneManager.sceneLoaded += ClearPools;

        pools.Add(poolObject, objects.ToList());
        return objects.ToList();

    }

    static void ClearPools(Scene scene, LoadSceneMode mode){
        pools.Clear();
    }

    /// <summary>
    /// Re-instantiate a object out of a pool.
    /// </summary>
    /// <param name="poolObject"> Reference of object </param>
    /// <param name="position"> The optional position where the object needs to instantiate </param>
    /// <param name="rotation"> The optional eulerAngles of the instantiated object </param>
    /// <returns></returns>
    public static PoolObject RespawnObject(PoolObject poolObject, Vector3 position = default, Vector3 rotation = default){

        if (pools.TryGetValue(poolObject, out List<PoolObject> objects)){

            PoolObject objectToSpawn = objects[0];

            objectToSpawn.OnRespawn();

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.eulerAngles = rotation;

            objectToSpawn.gameObject.SetActive(true);

            objects.RemoveAt(0);
            objects.Add(objectToSpawn);

            return objectToSpawn;

        }

        return null;

    }

    public static PoolObject RespawnObject(PoolObject poolObject, Vector3 position = default, Quaternion rotation = default) {

        if (pools.TryGetValue(poolObject, out List<PoolObject> objects)){

            PoolObject objectToSpawn = objects[0];

            objectToSpawn.OnRespawn();

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            objectToSpawn.gameObject.SetActive(true);

            objects.RemoveAt(0);
            objects.Add(objectToSpawn);

            return objectToSpawn;

        }

        return null;

    }

}
