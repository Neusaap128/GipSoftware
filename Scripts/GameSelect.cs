
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelect : MonoBehaviour
{

    private void Start(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadScene(int index) {
        SceneManager.LoadScene(index);
    }

}
