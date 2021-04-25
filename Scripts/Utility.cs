
using UnityEngine;
using System.Collections;

public static class Utility{

    public static T RandomEnumElement<T>() where T : System.Enum {
        System.Array values = System.Enum.GetValues(typeof(T));
        int r = Random.Range(0, values.Length);
        return (T)values.GetValue(r);
    }

    public static Vector2Int RoundVector2(this Vector2 vec){
        return new Vector2Int(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
    }

    public static GameObject LaunchInstantiateObject(GameObject gameObject, Vector3 position, Quaternion rotation, Vector3 spawnForce){
        GameObject go = GameObject.Instantiate(gameObject, position, rotation);
        go.GetComponent<Rigidbody>().AddForce(spawnForce, ForceMode.Impulse);
        return go;
    }

    public static Quaternion RotationWithRandomSpread(this Quaternion initialRotation, float theta){
        return initialRotation * Quaternion.Euler(new Vector3(0, 0, Random.Range(-theta, theta)));
    }

    public static bool DoesPositionLieInCone(Vector3 conePosition, Vector3 forward, float theta, Vector3 targetPosition, float maxRange = Mathf.Infinity){
        float angleToTarget = Vector3.Angle(forward, targetPosition - conePosition);
        if (Vector3.Distance(conePosition, targetPosition) > maxRange)
            return false;
        return angleToTarget <= theta && angleToTarget >= -theta;
    }

    public static void SetActive(this MonoBehaviour monoBehaviour, bool state, float time){
        monoBehaviour.StartCoroutine(SetActiveI(monoBehaviour.gameObject, state, time));
    }

    static IEnumerator SetActiveI(GameObject gameObject, bool state, float time){
        yield return new WaitForSeconds(time);
        gameObject.SetActive(state);
    }

}
