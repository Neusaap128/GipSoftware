using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Snake{

    public class Player : MonoBehaviour
    {


        List<GameObject> segments = new List<GameObject>();

        public GameObject segment;

        public float movementSpeed;
        float currentMovementSpeed;

        float inputHor;
        float inputVer;

        void Start(){
            segments.Add(transform.GetChild(0).gameObject);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
                inputHor = -1;
                inputVer = 0;
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
                inputHor = 1;
                inputVer = 0;
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
                inputVer = 1;
                inputHor = 0;
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
                inputVer = -1;
                inputHor = 0;
            }

            if (currentMovementSpeed <= 0){
                Move();
                currentMovementSpeed = movementSpeed;
            }else{
                currentMovementSpeed -= Time.deltaTime;
            }

        }

        void Move()
        {

            for (int i = segments.Count - 1; i >= 0; i--){

                if (i == 0){
                    segments[i].transform.position = transform.position;
                }else{
                    segments[i].transform.position = segments[i - 1].transform.position;
                }
            }

            if (inputHor > 0f){
                transform.position += Vector3.right;
            }else if (inputVer == 0){
                transform.position -= Vector3.right;
            }else if (inputVer > 0){
                transform.position += Vector3.up;
            }else{
                transform.position -= Vector3.up;
            }


        }

        void OnTriggerEnter2D(Collider2D collision){

            if (collision.gameObject.layer == LayerMask.NameToLayer("Fruit")){
                AddSegment();
                GameManager.instance.SpawnFruit();
                Destroy(collision.gameObject);
            }else if (collision.gameObject.layer == LayerMask.NameToLayer("Snake")){
                Die();
            }
        }

        void AddSegment()
        {
            GameObject go = null;
            if (inputHor > 0f){
                go = Instantiate(segment, new Vector2(transform.position.x - 1, transform.position.y), Quaternion.identity);
            }else if (inputVer == 0){
                go = Instantiate(segment, new Vector2(transform.position.x + 1, transform.position.y), Quaternion.identity);
            }else if (inputVer > 0){
                go = Instantiate(segment, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
            }else{
                go = Instantiate(segment, new Vector2(transform.position.x, transform.position.y - 1), Quaternion.identity);
            }
            segments.Add(go);
        }

        void Die(){
            Debug.Log("Ge zei dod");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

}