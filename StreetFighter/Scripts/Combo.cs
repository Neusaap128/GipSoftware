
using UnityEngine;

[CreateAssetMenu(fileName = "Combo", menuName = "StreetFigher/Combo")]
public class Combo : ScriptableObject
{

    public Attack[] requiredAttacks;
    public Attack attack;

}
