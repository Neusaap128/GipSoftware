
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace StreetFighter{

    [SelectionBase]
    public class PlayerCharacter : MonoBehaviour
    {

        PlayerStats playerStats;

        public int player;
        [Space]
        [HideInInspector]
        public Animator anim;
        [SerializeField] int punchAnims = 1;

        [HideInInspector]
        public Rigidbody2D rb;
        new BoxCollider2D collider;
        [Header("Movement")]
        [SerializeField] float speed = 10;
        [SerializeField] float jumpForce = 10;
        [SerializeField] float fallGravityMultiplier = 1.5f;
        [SerializeField] Transform groundCheckTransform;
        [SerializeField] float groundCheckRange;
        float timeSinceLastJump = 0;
        float timeSinceLastAttack = 0;

        [HideInInspector]
        public bool stunned;
        [HideInInspector]
        public float stunTime;

        [Header("Attacking")]
        [SerializeField] Transform attackTransform;
        [SerializeField] float attackRange = 2f;

        [SerializeField] Attack lightAttack;
        [SerializeField] Attack heavyAttack;

        bool windingUpForAttack;

        [SerializeField] Combo[] allCombos;
        List<Attack> previousAttacks = new List<Attack>();

        void Start() {
            rb = GetComponent<Rigidbody2D>();
            collider = GetComponent<BoxCollider2D>();
            playerStats = GetComponent<PlayerStats>();
            anim = GetComponentInChildren<Animator>();
        }

        bool moving = false;
        [HideInInspector]
        public bool blocking;
        void Update() {

            anim.SetBool("Moving", windingUpForAttack ? false : moving);
            blocking = Input.GetKey(player == 1 ? KeyCode.Q : KeyCode.Delete);
            anim.SetBool("Blocking", blocking);
            if (!stunned && playerStats.alive) {

                if (!windingUpForAttack) {
                    if (blocking)
                        return;

                    Move();

                    timeSinceLastJump += Time.deltaTime;
                    timeSinceLastAttack += Time.deltaTime;

                    //combat
                    bool performLightAttack = Input.GetKeyDown((player == 1) ? KeyCode.E : KeyCode.Space);
                    bool performHeavyAttack = Input.GetKeyDown((player == 1) ? KeyCode.Z : KeyCode.PageDown);

                    if (performLightAttack)
                        StartAttack(lightAttack);

                    if (performHeavyAttack)
                        StartAttack(heavyAttack);

                }

            }else{

                stunTime -= Time.deltaTime;
                if (stunTime <= 0)
                    stunned = false;

            }
        }

        void Move()
        {

            bool isGrounded = Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundCheckRange);
            anim.SetBool("IsGrounded", isGrounded);

            float inputHor = (player == 1) ? Input.GetAxis("HorizontalArrows") : Input.GetAxis("Horizontal");
            rb.position += Vector2.right * inputHor * speed * Time.deltaTime;

            bool jump = Input.GetKeyDown((player == 1) ? KeyCode.UpArrow : KeyCode.W);
            moving = inputHor != 0;

            if (rb.velocity.y < 0){
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallGravityMultiplier - 1) * Time.deltaTime;
            }

            if (jump && timeSinceLastJump > 0.2f)
            {
                if (isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    timeSinceLastJump = 0;
                    anim.SetTrigger("Jump");
                }
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

        void StartAttack(Attack attack){

            if (windingUpForAttack)
                return;

            windingUpForAttack = true;
            StartCoroutine(Attack(attack, attack.windUpTime));
        }

        IEnumerator Attack(Attack attack, float time){
            yield return new WaitForSeconds(time);
            PlayerStats enemy = FindPlayer()?.playerStats;
            anim.SetTrigger(attack.animationTrigger);

            if(enemy != null){
                enemy.TakeDamage(attack.damage, attackTransform, attack.knockbackForce);
                Stun(attack.afterTime);
            }else{
                //miss stun
                Stun(attack.missStunTime);
            }

            if (previousAttacks.Count >= 5)
                previousAttacks.RemoveAt(0);

            previousAttacks.Add(attack);
            windingUpForAttack = false;

            CheckCombos();

        }

        void CheckCombos() {
            foreach (Combo combo in allCombos){

                if (previousAttacks.Count < combo.requiredAttacks.Length)
                    continue;

                bool performCombo = true;
                for (int i = 0; i < combo.requiredAttacks.Length; i++){
                    if (previousAttacks[previousAttacks.Count-1 - i] != combo.requiredAttacks[combo.requiredAttacks.Length-1 - i])
                    {
                        performCombo = false;
                        break;
                    }
                }

                if(performCombo)
                    StartAttack(combo.attack);
                

            }
        }

        public void Stun(float s) {
            moving = false;
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