
using UnityEngine;

namespace RiskOfRain {

    public class Portal : Interactable
    {

        protected override void OnCollision(PlayerStats player){
            StartCoroutine(EnemySpawner.EndLevel());
        }

    }
}