using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager instance;

        public GameObject fruit;

        private void Awake()
        {
            instance = this;
        }

        public void SpawnFruit()
        {
            int randX = Random.Range(-10, 10);
            int randY = Random.Range(-5, 5);

            Instantiate(fruit, new Vector2(randX, randY), Quaternion.identity, transform);

        }

    }

}