
using UnityEngine;

namespace RiskOfRain {

    [CreateAssetMenu(fileName = "Attack", menuName = "Shooter/Player/Attack")]
    public class Attack : ScriptableObject
    {
    
        public float damage;
        public float fireRate;
        public float inaccuracy;
        [Space]
        public PlayerBullet bulletPrefab;
    
    }

}