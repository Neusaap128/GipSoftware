
using UnityEngine;

public static class Utility{

    public static T RandomEnumElement<T>() where T : System.Enum {
        System.Array values = System.Enum.GetValues(typeof(T));
        int r = Random.Range(0, values.Length);
        return (T)values.GetValue(r);
    }

    public static Vector2Int RoundVector2(this Vector2 vec){
        return new Vector2Int(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
    }

}
