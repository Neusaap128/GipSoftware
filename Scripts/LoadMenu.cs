
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{

    private void Start(){
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){

        if (scene.buildIndex == 0){
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }

    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.P)) {
            SceneManager.LoadScene(0);
        }
    }
}
