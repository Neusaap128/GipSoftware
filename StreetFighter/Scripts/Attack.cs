
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "StreetFigher/Attack")]
public class Attack : ScriptableObject
{

    public int damage;
    public float knockbackForce;
    public float windUpTime;
    public float afterTime;
    public float missStunTime;
    public string animationTrigger;

}
