
using UnityEngine;

namespace RiskOfRain {

    [RequireComponent(typeof(Collider)), SelectionBase]
    public abstract class Interactable : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision) {
            if(collision.TryGetComponent(out PlayerStats stats)){
                OnCollision(stats);
            }
        }

        protected abstract void OnCollision(PlayerStats player);

    }

}