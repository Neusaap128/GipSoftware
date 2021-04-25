
using UnityEngine;

namespace RiskOfRain
{
    public class Enemy : HitAble
    {
    
        [Header("Enemy")]
        public TerrainBiomes biomes;

        [SerializeField] float walkSpeed;
        [SerializeField] float jumpForce;
        [SerializeField] float wallDetectRange;
        [Space]
        [SerializeField] int damage;
        [SerializeField] float attackSpeed;
        float currentAttackSpeed;
        [SerializeField] float attackDistance;
        Rigidbody2D rb;

        [HideInInspector] public PlayerStats player1, player2;
        [SerializeField] Transform feetPosition;
        [Space]
        public int powerScore;

        Vector3 moveVector = Vector3.right;

        private void Start(){
            rb = GetComponent<Rigidbody2D>();
            currentHealth = maxHealth;
        }

        void FixedUpdate(){
            Move();
        }
        
        protected virtual void Move(){

            bool player1Visible =   Utility.DoesPositionLieInCone(transform.position, Vector3.right, 30, player1.transform.position, 10) ||
                                    Utility.DoesPositionLieInCone(transform.position, Vector3.left,  30, player1.transform.position, 10);
            bool player2Visible =   Utility.DoesPositionLieInCone(transform.position, Vector3.right, 30, player2.transform.position, 10) ||
                                    Utility.DoesPositionLieInCone(transform.position, Vector3.left,  30, player2.transform.position, 10);

            if (player1Visible){
                MoveToPlayer(player1);
            }else if (player2Visible){
                MoveToPlayer(player2);
            }else{
                Wander();
            }

        }

        void MoveToPlayer(PlayerStats player){

            currentAttackSpeed -= Time.deltaTime;
            if(Vector2.Distance(player.transform.position, transform.position) <= attackDistance){
                if(currentAttackSpeed <= 0.0f){
                    Attack(player);
                    currentAttackSpeed = 1/attackSpeed;
                }

            }else{
                moveVector = new Vector2((player.transform.position - transform.position).x,0).normalized;
                transform.position += moveVector * walkSpeed * Time.deltaTime;
                if (IsGrounded()){
                    if (IsHittingWall(moveVector)){
                        transform.position += Vector3.up * jumpForce;
                    }
                }
            }

        }

        protected virtual void Attack(PlayerStats player){
            player.TakeDamage(damage);
            //play animation
        }

        float timeBtwRandDir = 5f;
        void Wander(){

            if (moveVector != Vector3.zero){
                if (IsGrounded()){
                    if (IsHittingWall(moveVector)){
                        transform.position += Vector3.up * jumpForce;
                    }
                }
            }

            if (timeBtwRandDir <= 0.0f){
                moveVector = Random.value < 0.5f ? Vector3.left : Vector3.right;
                timeBtwRandDir = Random.Range(5f, 6f);
            }else if (timeBtwRandDir <= 1.5f){
                moveVector = Vector3.zero;
            }
            timeBtwRandDir -= Time.deltaTime;

            transform.position += moveVector * walkSpeed * Time.deltaTime;
            

        }

        protected override void Die(){
            EnemySpawner.RemoveEnemy(this);
            Destroy(gameObject);
        }

        bool IsGrounded(){
            return Physics2D.Raycast(feetPosition.position + Vector3.down * 0.1f, Vector3.down, 0.1f);
        }
        
        bool IsHittingWall(Vector2 dir){
            RaycastHit2D[] hits = Physics2D.RaycastAll(feetPosition.position, dir, wallDetectRange);
            foreach (RaycastHit2D item in hits){
                if (item.collider.gameObject.layer == LayerMask.NameToLayer("Ground")){
                    return true;
                }
            }
            return false;   
        }

        private void OnDrawGizmosSelected(){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(feetPosition.position, feetPosition.position + moveVector * wallDetectRange);
            Gizmos.DrawLine(feetPosition.position + Vector3.down * 0.1f, feetPosition.position + Vector3.down * 0.2f);
        }
    }
}