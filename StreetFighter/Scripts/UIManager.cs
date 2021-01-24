
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace StreetFighter{

    public class UIManager : MonoBehaviour
    {

        private static UIManager instance;

        [SerializeField] Slider leftHealthBarPlayer;
        [SerializeField] Slider rightHealthBarPlayer;

        [SerializeField] Image KoImage;
        [SerializeField] Text victoryText;

        public void Awake(){
            if(instance == null)
                instance = this;
        }

        public static void SetHeathBarValue(int player, int amount){

            if(player == 1){
                instance.leftHealthBarPlayer.value = amount;
            }else {
                instance.rightHealthBarPlayer.value = amount;
            }
        }
        
        public static IEnumerator EnableVictoryUI(int player){

            instance.KoImage.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);

            instance.KoImage.gameObject.SetActive(false);

            instance.victoryText.text = (player == 1 ? "Ken" : "Ryu") + " Has won"; 
            instance.victoryText.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);

            instance.victoryText.text = "Press Top Button to restart";

        }

        
    }
}