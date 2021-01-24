
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PacMan{

    public class UIManager : MonoBehaviour
    {

        static UIManager instance;

        [SerializeField] Text winText;

        public static int aliveGhosts = 4;

        private void Start(){
            if (instance == null)
                instance = this;
        }

        public static IEnumerator EnableWinText(){
            instance.winText.gameObject.SetActive(true);
            instance.winText.text = "Pac Man Won";
            yield return new WaitForSeconds(3f);
            instance.winText.text = "Press Top Button to restart";
        }

        public static IEnumerator EnableLostText(){
            instance.winText.gameObject.SetActive(true);
            instance.winText.text = "Pac Man Lost";
            yield return new WaitForSeconds(3f);
            instance.winText.text = "Press Top Button to restart";
        }


    }
}