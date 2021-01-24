
using UnityEngine;

namespace RiskOfRain {

    [CreateAssetMenu(fileName = "Attack")]
    public class Attack : ScriptableObject
    {
    
        public float damage;
        public float fireRate;
        [Space]
        public GameObject bulletPrefab;
    
    }

}