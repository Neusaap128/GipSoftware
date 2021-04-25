using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyer : MonoBehaviour{

    [SerializeField] float lifetime;

    private void Start(){
        Destroy(gameObject, lifetime);
    }

}
