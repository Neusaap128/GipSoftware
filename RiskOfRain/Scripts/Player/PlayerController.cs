
using UnityEngine;
using System.Collections.Generic;

namespace RiskOfRain {

    public class PlayerController : MonoBehaviour
    {
    
        Rigidbody2D rb;
        PlayerStats playerStats;

        [SerializeField] float speed;
        [SerializeField] float jumpForce;
        [SerializeField] Transform groundCheckPosition;
        [Space]
        [SerializeField] float fallMultiplier;
        [SerializeField] float lowJumpMultiplier;
        [Space]
        [SerializeField] float rotateSpeed;
        [SerializeField] Transform gun;
        public int player = 0;
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

            if (Input.GetKey(player == 1 ? KeyCode.PageDown : KeyCode.E)){
                angle -= rotateSpeed * Time.deltaTime;
                Vector2 dir = new Vector2(Mathf.Cos(angle*Mathf.Deg2Rad), Mathf.Sin(angle*Mathf.Deg2Rad));
                gun.transform.position = (Vector2)transform.position + dir * 0.25f;
                gun.eulerAngles = new Vector3(0, 0, angle-90);

            }else if (Input.GetKey(player == 1 ? KeyCode.Delete : KeyCode.Q)){
                angle += rotateSpeed * Time.deltaTime;
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                gun.transform.position = (Vector2)transform.position + dir * 0.25f;
                gun.eulerAngles = new Vector3(0, 0, angle-90);
            }

            #endregion

            if (playerStats.currentTimePrimaryFireRate <= 0){
                if (Input.GetKey(player == 1 ? KeyCode.Space : KeyCode.Z)){
                    Shoot();
                    playerStats.currentTimePrimaryFireRate = 1/playerStats.primaryAttack.fireRate;
                }
            }else{
                playerStats.currentTimePrimaryFireRate -= Time.deltaTime;
            }
        }

        void Shoot(){
            PoolManager.RespawnObject(playerStats.bulletPrefab, firePoint.position, firePoint.rotation);
        }

    }

}