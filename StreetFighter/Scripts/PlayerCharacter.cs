
using UnityEngine;

namespace StreetFighter{

    public class PlayerCharacter : MonoBehaviour
    {

        PlayerStats playerStats;

        public int player;
        [Space]
        [HideInInspector]
        public Animator anim;
        [SerializeField] private int punchAnims = 1;

        [HideInInspector]
        public Rigidbody2D rb;
        new BoxCollider2D collider;
        [Header("Movement")]
        [SerializeField] private float speed = 10;
        [SerializeField] private float jumpForce = 10;
        [SerializeField] private float fallGravityMultiplier = 1.5f;
        [SerializeField] private Transform groundCheckTransform;
        [SerializeField] private float groundCheckRange;
        private float timeSinceLastJump = 0;
        private float timeSinceLastAttack = 0;

        [HideInInspector]
        public bool stunned;
        [HideInInspector]
        public float stunTime;

        [Header("Attacking")]
        public Transform attackTransform;
        public float attackRange = 2f;

        [HideInInspector]
        public bool windUp = false;
        [HideInInspector]
        public float currentAttackWindUp;
        public float attackLightWindUpTime = 0.3f;
        public float attackHeavyWindUpTime = 0.5f;

        void Start() {

            rb = GetComponent<Rigidbody2D>();
            collider = GetComponent<BoxCollider2D>();
            playerStats = GetComponent<PlayerStats>();
            currentAttackWindUp = attackLightWindUpTime;
            anim = GetComponentInChildren<Animator>();
        }

        
        int attack = 0;
        bool moving = false;
        [HideInInspector]
        public bool blocking;
        void Update() {

            anim.SetBool("Moving", moving);
            blocking = Input.GetKey(player == 1 ? KeyCode.Q : KeyCode.Delete);
            anim.SetBool("Blocking", blocking);
            if (!stunned && playerStats.alive){

                bool isGrounded = Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundCheckRange);
                anim.SetBool("IsGrounded", isGrounded);
                if (!windUp){
                    if (blocking) return;
                    //movement
                    float inputHor = (player == 1) ? Input.GetAxis("HorizontalArrows") : Input.GetAxis("Horizontal");
                    rb.position += Vector2.right * inputHor * speed * Time.deltaTime;

                    bool jump = Input.GetKeyDown((player == 1) ? KeyCode.UpArrow : KeyCode.W);
                    moving = inputHor != 0;

                    if (jump && timeSinceLastJump > 0.2f){
                        if (isGrounded){
                            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                            timeSinceLastJump = 0;
                            anim.SetTrigger("Jump");
                        }
                    }

                }

                if (rb.velocity.y < 0){
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (fallGravityMultiplier - 1) * Time.deltaTime;
                }

                timeSinceLastJump += Time.deltaTime;
                timeSinceLastAttack += Time.deltaTime;

                //combat
                bool lightAttack = (player == 1) ? Input.GetKeyDown(KeyCode.E) : Input.GetKeyDown(KeyCode.Space);
                bool heavyAttack = (player == 1) ? Input.GetKeyDown(KeyCode.Z) : Input.GetKeyDown(KeyCode.PageDown);

                //checking attack input
                if (!windUp && timeSinceLastAttack >= 0.5f)
                {
                    if (lightAttack) {
                        attack = 4;
                        windUp = true;
                        anim.SetTrigger("CrouchKickSweep");
                        currentAttackWindUp = 0.05f;
                        timeSinceLastAttack = 0;
                    }

                    if (timeSinceLastJump <= 0.2f && heavyAttack)
                    {
                        attack = 3;
                        windUp = true;
                        anim.SetTrigger("JumpKick");
                        currentAttackWindUp = 0.05f;
                        timeSinceLastAttack = 0;
                    }

                    if (timeSinceLastJump <= 0.2f && lightAttack)
                    {
                        attack = 3;
                        windUp = true;
                        anim.SetTrigger("Uppercut");
                        currentAttackWindUp = 0.1f;
                        timeSinceLastAttack = 0;
                    }

                    if (timeSinceLastJump >= 0.2f && lightAttack)
                    {
                        if (isGrounded)
                        {
                            windUp = true;
                            attack = 0;
                            currentAttackWindUp = attackLightWindUpTime;
                            int punch = Random.Range(1, punchAnims + 1);
                            string punchStr = "Punch0" + punch.ToString();
                            anim.SetTrigger(punchStr);
                            timeSinceLastAttack = 0;
                        }
                    }

                    if (timeSinceLastJump >= 0.2f && heavyAttack)
                    {
                        if (isGrounded)
                        {
                            windUp = true;
                            attack = 1;
                            currentAttackWindUp = attackHeavyWindUpTime;
                            anim.SetTrigger("SpecialAttack");
                            timeSinceLastAttack = 0;
                        }

                    }
                }

                if (windUp){
                    if (currentAttackWindUp <= 0.0f){
                        Attack(attack);
                    }else{
                        currentAttackWindUp -= Time.deltaTime;
                    }
                }

            }else{

                moving = false;
                stunTime -= Time.deltaTime;
                if (stunTime <= 0) stunned = false;
            }
        }

        PlayerCharacter FindPlayer(){
            Collider2D[] collisions = Physics2D.OverlapCircleAll(attackTransform.position, attackRange);
            for (int i = 0; i < collisions.Length; i++){
                if (collisions[i] == collider) continue;

                if (collisions[i].CompareTag("Player")){
                    return collisions[i].GetComponent<PlayerCharacter>();
                }
            }

            return null;
        }

        void Attack(int type){
            PlayerCharacter enemy = FindPlayer();
            if(enemy != null){
                switch (type){
                    case 0:
                        enemy.playerStats.TakeDamage(10, transform, 1f);
                        break;
                    case 1:
                        enemy.playerStats.TakeDamage(20, transform, 1f);
                        break;
                    case 2:
                        enemy.playerStats.TakeDamage(20, transform, 1.5f);
                        break;
                    case 3:
                        enemy.playerStats.TakeDamage(30, transform, 2f);
                        break;
                    case 4:
                        enemy.playerStats.TakeDamage(30, transform, 2f);
                        break;

                }
            }else{
                //miss stun
                Stun(0.5f);
            }
            windUp = false;
        }

        public void Stun(float s) {
            stunned = true;
            stunTime = s;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackTransform.position, attackRange);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(groundCheckTransform.position, transform.position + Vector3.down * groundCheckRange);
        }

    }

}