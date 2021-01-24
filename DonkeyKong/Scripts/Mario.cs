
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DonkeyKong{

    public class Mario : MonoBehaviour{

        public static Mario instance;

        Rigidbody2D rb;
        [SerializeField] float speed;
        [SerializeField] float climbSpeed;
        [SerializeField] float jumpForce;
        [Space]
        [SerializeField] Transform groundCheck;
        [SerializeField] float groundCheckDistance;
        [Space]
        [SerializeField] Text scoreText;
        [Space]
        [SerializeField] BoxCollider2D boxCollider;

        SpriteRenderer SR;
        bool facingRight = true;

        bool alive = true;

        bool won;

        public static bool Alive => instance.alive;
        public static bool Won => instance.won;

        bool climbing;

        bool overLadder;
        float originalGravity;

        Animator anim;


        void Start(){
            instance = this;
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            SR = GetComponent<SpriteRenderer>();
        }
        
        void FixedUpdate(){

            if (!alive)
                return;

            if(rb.position.y >= 4){
                Win();
            }

            anim.SetBool("Climbing", climbing);

            #region movement
            if (climbing){
                float verInput = Input.GetAxisRaw("Vertical");
                transform.position += Vector3.up * verInput * climbSpeed* Time.deltaTime;
                
                if (!overLadder){
                    StopClimb();
                }
                
            }else{

                float horInput = Input.GetAxisRaw("Horizontal");
                transform.position += Vector3.right * horInput * speed * Time.deltaTime;

                if (horInput < 0 && facingRight) FlipPlayer();
                if (horInput > 0 && !facingRight) FlipPlayer();

                anim.SetBool("Run", horInput != 0);
            }
            #endregion

        }

        private void Update(){

            if (!alive || won)
                return;

            if (Input.GetKeyDown(KeyCode.W)){

                if (overLadder){
                    StartClimb();
                }else{
                    if(Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance))
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }

        }

        void StartClimb(){
            originalGravity = rb.gravityScale;
            rb.gravityScale = 0;
            climbing = true;
            boxCollider.isTrigger = true;
        }

        void StopClimb() {
            climbing = false;
            rb.gravityScale = 1;
            boxCollider.isTrigger = false;
        }

        private void OnTriggerEnter2D(Collider2D collision){
            if(collision.gameObject.layer == LayerMask.NameToLayer("Ladder")){
                overLadder = true;
            }else if(collision.gameObject.layer == LayerMask.NameToLayer("Barrel")){
                Die();
            }
        }

        private void OnTriggerExit2D(Collider2D collision){
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ladder")){
                overLadder = false;
            }
        }

        void Win(){
            won = true;
            Debug.Log("you Won");
        }

        void Die()
        {
            if (climbing) StopClimb();
            alive = false;
            anim.SetBool("Alive", false);
            Invoke("ReloadScene", 3);
            Debug.Log("Player be dead");
        }

        void FlipPlayer(){
            facingRight = !facingRight;
            SR.flipX = !SR.flipX;

        }

        void ReloadScene(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnDrawGizmosSelected(){
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }

    }
}