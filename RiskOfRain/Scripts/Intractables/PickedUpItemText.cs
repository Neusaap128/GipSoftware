
using UnityEngine;
using UnityEngine.UI;

public class PickedUpItemText : MonoBehaviour
{

    static PickedUpItemText instance;
    Text text;

    private void Awake(){
        instance = this;
    }

    private void Start(){
        text = GetComponentInChildren<Text>();
        text.gameObject.SetActive(false);
    }

    public static void SetPostionAndText(Vector3 position, string text){
        instance.text.gameObject.SetActive(true);
        instance.transform.position = position + Vector3.up * 0.5f;
        instance.text.text = text;
        instance.Invoke("SetInactive", 4f);
    }

    void SetInactive(){
        instance.text.gameObject.SetActive(false);
    }

}
