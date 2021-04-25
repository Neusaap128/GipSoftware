
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StreetFighter {

    public class PlayerStats : MonoBehaviour
    {

        public int maxHealth;
        public int currentHealth;

        private PlayerCharacter playerCharacter;

        public bool alive = true;

        void Start(){
            currentHealth = maxHealth;
            playerCharacter = GetComponent<PlayerCharacter>();
            UIManager.SetHeathBarValue(playerCharacter.player, currentHealth);
        }

        private void Update(){
            if (Input.GetKeyDown(KeyCode.Q))
                if (!alive)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void TakeDamage(int amount, Transform transform, float knockBackForce) {

            if (playerCharacter.blocking || !alive)
                return;

            if (playerCharacter.stunned){
                amount = Mathf.RoundToInt(amount * 1.5f);
            }

            currentHealth -= amount;

            UIManager.SetHeathBarValue(playerCharacter.player, currentHealth);

            if (currentHealth <= 0) {
                Die();
            }

            playerCharacter.anim.SetTrigger("Hit");

            Vector2 position = transform.position;
            Vector2 horForce = (playerCharacter.rb.position - position) * knockBackForce;
            Vector2 verForce = Vector2.up * 2f;
            Vector2 force = horForce + verForce;
            playerCharacter.rb.AddForce(force, ForceMode2D.Impulse);

            playerCharacter.Stun(0.2f);

        }

        public void Die() {
            alive = false;
            playerCharacter.anim.SetTrigger("Die");
            StartCoroutine(UIManager.EnableVictoryUI(playerCharacter.player == 1 ? 2 : 1));
            Debug.Log(gameObject.name + " is dead");
        }

    }
}