
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PacMan{

    public class Player : MonoBehaviour {

        Animator anim;

        Vector2 dir = Vector2.right;
        public float speed = 0.25f;
        float currentTimeBtwMove;

        [HideInInspector]
        public bool alive = true;
        [HideInInspector] public bool invulnerable = false;
        float invulnerableTimer = 0;

        bool won = false;

        public static event System.Action<bool> onInvulnerableStateChanged;

        void Start(){
            anim = GetComponent<Animator>();
            invulnerable = false;
        }
       
        void Update(){

            if (!alive || won){

                if (Input.GetKeyDown(KeyCode.Q))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
                return;
            }

            if (invulnerable) { 
                if (invulnerableTimer <= 0){
                    invulnerable = false;
                    onInvulnerableStateChanged?.Invoke(false);
                }else{
                    invulnerableTimer -= Time.deltaTime;
                }
            }

            CheckInput();

            RaycastHit2D collided = Physics2D.Raycast((Vector2)transform.position + dir * 0.5f, dir, 0.2f);

            if (collided.collider != null){
                if (collided.collider.gameObject.layer == LayerMask.NameToLayer("Fruit")){
                    Destroy(collided.collider.gameObject);
                    SetInvulnerable();
                }
            }

            anim.SetBool("Moving", !collided);
            if(!collided)
                transform.position += (Vector3)dir * speed * Time.deltaTime;


        }

        void SetInvulnerable(){

            Debug.Log("Set invulnerable");
            invulnerable = true;
            invulnerableTimer = 5f;
            onInvulnerableStateChanged?.Invoke(true);
        }

        void CheckInput(){

            if (Input.GetKeyDown(KeyCode.W)){
                TryChangeDirection(Vector2.up);
            }else if (Input.GetKeyDown(KeyCode.D)){
                TryChangeDirection(Vector2.right);
            }else if (Input.GetKeyDown(KeyCode.S)){
                TryChangeDirection(Vector2.down);
            }else if (Input.GetKeyDown(KeyCode.A)){
                TryChangeDirection(Vector2.left);
            }

        }

        private void TryChangeDirection(Vector2 direction){
            if (!Physics2D.Raycast((Vector2)transform.position + direction * 0.5f, direction, 0.2f)){
                dir = direction;
                transform.rotation = Quaternion.Euler(Vector3.forward * Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
                transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            }
        }

        public void Win(){
            won = true;
            Debug.Log("Pac Man won");
            StartCoroutine(UIManager.EnableWinText());
        }
        
        public void Die(){

            if (won)
                return;

            anim.SetBool("Dead", true);
            
            Debug.Log("Pac Man Died");
            StartCoroutine(UIManager.EnableLostText());

            alive = false;
        }

    }
}