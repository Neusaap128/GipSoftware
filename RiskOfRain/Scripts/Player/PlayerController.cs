
using UnityEngine;
using System.Collections.Generic;

namespace RiskOfRain {

    public class PlayerController : MonoBehaviour
    {
    
        Rigidbody2D rb;
        PlayerStats playerStats;

        public float speed;
        public float jumpForce;
        [SerializeField] Transform groundCheckPosition;
        [Space]
        [SerializeField] float fallMultiplier;
        [SerializeField] float lowJumpMultiplier;
        [Space]
        [SerializeField] float rotateSpeed;
        [SerializeField] Transform gun;
        public enum Player { player1, player2 }
        public Player player = Player.player1;
        [Space]
        [SerializeField] Transform firePoint;

        void Start(){
            playerStats = GetComponent<PlayerStats>();
            rb = GetComponent<Rigidbody2D>();
        }

        float angle = 0;

        void Update(){

            #region Movement

            float inputHor = Input.GetAxis(player == 0 ? "HorizontalArrows" : "Horizontal");
            transform.position += Vector3.right * inputHor * speed * Time.deltaTime;

            bool isGrounded = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, 0.1f);

            float inputVer = Input.GetAxisRaw(player == 0 ? "VerticalArrows" : "Vertical");
            if (inputVer > 0){
                if(isGrounded)
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }    

            if(rb.velocity.y < 0){
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }else if(rb.velocity.y > 0 && inputVer == 0){
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            #endregion

            #region Rotating Weapon
        
            if (Input.GetKey(player == Player.player1 ? KeyCode.PageDown : KeyCode.E)){
                angle -= rotateSpeed * Time.deltaTime;
                Vector2 dir = new Vector2(Mathf.Cos(angle*Mathf.Deg2Rad), Mathf.Sin(angle*Mathf.Deg2Rad));
                gun.transform.position = (Vector2)transform.position + dir * 0.25f;
                gun.eulerAngles = new Vector3(0, 0, angle-90);

            }else if (Input.GetKey(player == Player.player1 ? KeyCode.Delete : KeyCode.Q)){
                angle += rotateSpeed * Time.deltaTime;
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                gun.transform.position = (Vector2)transform.position + dir * 0.25f;
                gun.eulerAngles = new Vector3(0, 0, angle-90);
            }

            #endregion

            if (playerStats.currentTimePrimaryFireRate <= 0){
                if (Input.GetKey(player == Player.player1 ? KeyCode.Space : KeyCode.Z)){
                    Shoot(playerStats, playerStats.primaryAttack);
                }
            }else{
                playerStats.currentTimePrimaryFireRate -= Time.deltaTime;
            }

            if (playerStats.currentTimeSecondaryFireRate <= 0){
                if (Input.GetKey(player == Player.player2 ? KeyCode.M : KeyCode.R)){
                    Shoot(playerStats, playerStats.secondaryAttack);
                }
            }else{
                playerStats.currentTimeSecondaryFireRate -= Time.deltaTime;
            }

        }

        void Shoot(PlayerStats stats, Attack attack){

            if (attack == stats.primaryAttack) {
                stats.currentTimePrimaryFireRate = 1 / (attack.fireRate + stats.totalFireRateAddition);
            }else{
                stats.currentTimeSecondaryFireRate = 1 / (attack.fireRate + stats.totalFireRateAddition);
            }

            Quaternion rotation = firePoint.rotation.RotationWithRandomSpread(attack.inaccuracy);
            PoolObject go = PoolManager.RespawnObject(attack.bulletPrefab, firePoint.position, rotation);

            go.GetComponent<PlayerBullet>().damage = attack.damage + stats.totalDamageAddition;

        }


    }

}