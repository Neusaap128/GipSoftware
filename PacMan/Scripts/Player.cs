
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PacMan{

    public class Player : MonoBehaviour {

        Animator anim;

        Vector2 dir = new Vector2(1, 0);
        public float speed = 0.25f;
        float currentTimeBtwMove;
        Vector2 oldDir = Vector2.zero;

        [HideInInspector]
        public bool alive = true;
        [HideInInspector] public bool invurnable = false;
        float invurnableTimer = 0;

        float startTimer = 4.5f;

        bool won = false;

        public static System.Action onInvurnable;
        public static System.Action onInvurnablePassed;

        void Start(){
            anim = GetComponent<Animator>();
            invurnable = false;
        }
       
        void Update(){

            if (!alive || won){

                if (Input.GetKeyDown(KeyCode.Q))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

                return;
            }

            if (invurnable) { 
                if (invurnableTimer <= 0){
                    invurnable = false;
                    onInvurnablePassed.Invoke();
                }else{
                    invurnableTimer -= Time.deltaTime;
                }
            }

            if (startTimer >= 0){
                startTimer -= Time.deltaTime;
                return;
            }

            CheckInput();

            RaycastHit2D canMove = Physics2D.Raycast((Vector2)transform.position + dir * 0.5f, dir, 0.2f);

            if (canMove.collider != null){
                if (canMove.collider.gameObject.layer == LayerMask.NameToLayer("Fruit")){
                    Destroy(canMove.collider.gameObject);
                    SetInvurnable();
                }
            }

            anim.SetBool("Moving", !canMove);
            if(!canMove)
                transform.position += (Vector3)dir * speed * Time.deltaTime;

            if (oldDir != dir){
                transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                oldDir = dir;
            }

        }

        void SetInvurnable(){

            invurnable = true;
            invurnableTimer = 5f;
            onInvurnable.Invoke();
        }

        void CheckInput(){

            if (Input.GetKeyDown(KeyCode.W) && !Physics2D.Raycast((Vector2)transform.position + Vector2.up * 0.5f, Vector2.up, 0.2f)){
                dir = Vector2.up; 
            }else if (Input.GetKeyDown(KeyCode.D) && !Physics2D.Raycast((Vector2)transform.position + Vector2.right * 0.5f, Vector2.right, 0.2f)){
                dir = Vector2.right;
            }else if (Input.GetKeyDown(KeyCode.S) && !Physics2D.Raycast((Vector2)transform.position + Vector2.down * 0.5f, Vector2.down, 0.2f)){
                dir = Vector2.down;
            }else if (Input.GetKeyDown(KeyCode.A) && !Physics2D.Raycast((Vector2)transform.position + Vector2.left * 0.5f, Vector2.left, 0.2f)){
                dir = Vector2.left;
            }

            transform.rotation = Quaternion.Euler(Vector3.forward * (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));

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