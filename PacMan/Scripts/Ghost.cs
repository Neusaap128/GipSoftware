
using UnityEngine;

namespace PacMan
{
    public class Ghost : MonoBehaviour
    {
        Animator anim;
        Vector2 dir = new Vector2(1, 0);
        [SerializeField] private float speed;

        bool vulnerable = false;

        [SerializeField] Player player;

        void Start(){

            Player.onInvulnerableStateChanged += OnPlayerInvulnerableStateChanged;
            anim = GetComponent<Animator>();
        }

        void Update(){

            RaycastHit2D[] colliders = Physics2D.RaycastAll(transform.position, dir, 0.6f);
            for (int i = 0; i < colliders.Length; i++) {

                if(colliders[i].collider.gameObject.layer == LayerMask.NameToLayer("Ground")){

                    int rand = Random.Range(0, 4);
                    switch (rand){
                        case 0:
                            dir = Vector2.right;
                            break;
                        case 1:
                            dir = Vector2.down;
                            break;
                        case 2:
                            dir = Vector2.left;
                            break;
                        case 3:
                            dir = Vector2.up;
                            break;
                    }
                    transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                }
            }

            transform.position += (Vector3)dir * speed * Time.deltaTime;

            SetAnim();

        }


        void OnPlayerInvulnerableStateChanged(bool state){
            Debug.Log("Ghost thingy");
            vulnerable = state;
            if(anim == null)
                anim = GetComponent<Animator>();
            anim.SetBool("Vulnerable", state);
        }

        public void SetAnim(){

            if (dir == Vector2.left) {
                anim.SetInteger("Direction", 0);
            } else if (dir == Vector2.up) {
                anim.SetInteger("Direction", 1);
            } else if (dir == Vector2.right) {
                anim.SetInteger("Direction", 2);
            } else if (dir == Vector2.down) {
                anim.SetInteger("Direction", 3);
            }


        }

        void Die(){

            UIManager.aliveGhosts--;
            if (UIManager.aliveGhosts == 0)
                player.Win();

            Player.onInvulnerableStateChanged -= OnPlayerInvulnerableStateChanged;
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision){

            if (collision.gameObject.layer == LayerMask.NameToLayer("Player")){

                if (vulnerable){
                    Die();
                }else{
                    player.Die();
                    
                }
            }
        }

    }
}