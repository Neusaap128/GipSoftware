
using UnityEngine;

namespace RiskOfRain
{
    public class Enemy : HitAble
    {
    
        public TerrainBiomes biomes;

        public int powerScore;

        void Update(){
            Move();
        }
    
        protected virtual void Move(){
            
        }
    
        protected override void Die(){
            EnemySpawner.RemoveEnemy(this);
            gameObject.SetActive(false);
        }
    }
}