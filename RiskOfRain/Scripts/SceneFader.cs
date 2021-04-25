
using UnityEngine;
using System.Collections;

namespace RiskOfRain { 

    public class SceneFader : MonoBehaviour
    {

        static SceneFader instance;

        static Animator anim;
    
        private void Start(){
            if (instance == null)
                instance = this;

            anim = GetComponent<Animator>();
        }
    
        public static void FadeIn(){
            anim.SetBool("Activated", true);
        }

        public static void FadeOut(){
            anim.SetBool("Activated", false);
        }
    
    }

}