
using UnityEngine;
using System.Collections.Generic;

namespace PacMan{

    public class Coins : MonoBehaviour
    {

        int coinsEaten = 0;

        [SerializeField] Transform[] coins;
        [SerializeField] Player player;
        [SerializeField] SpriteRenderer mapRenderer;

        private (Bounds b, List<int> coins)[] bounds;

        void Start(){

            coins = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++){
                coins[i] = transform.GetChild(i);
            }

            bounds = new (Bounds, List<int>)[6];

            Bounds b = mapRenderer.bounds;

            Vector3 size = new Vector3(b.size.x/3,b.size.y/2);

            int j = 0;
            for (int x = -1; x <= 1; x++){
                for (int y = -1; y < 1; y++){

                    Bounds _b = new Bounds(new Vector3(size.x * x, size.y * y + (size.y/2) + 0.1f), size);

                    List<int> indices = new List<int>();

                    for (int i = 0; i < coins.Length; i++){

                        if (_b.Contains(coins[i].transform.position))
                            indices.Add(i);

                    }

                    bounds[j] = (_b, indices);
                    j++;
                }
            }
        }

        void Update(){

            for (int i = 0; i < bounds.Length; i++){
                if (bounds[i].b.Contains(player.transform.position)){
                    for (int j = 0; j < bounds[i].coins.Count; j++) {

                        if (coins[bounds[i].coins[j]] == null)
                            continue;

                        if (Vector2.Distance(player.transform.position, coins[bounds[i].coins[j]].transform.position) < 0.1f){

                            coinsEaten++;
                            Destroy(coins[bounds[i].coins[j]].gameObject);

                            if (coinsEaten >= coins.Length){
                                player.Win();
                                gameObject.SetActive(false);
                                return;
                            }
                        }
                    }
                }
            }
        }

    }
}