
using UnityEngine;

namespace RiskOfRain{

    [CreateAssetMenu(fileName = "Item", menuName = "Shooter/Player/Item")]
    public class Item : ScriptableObject
    {
        public new string name;
        public int maxHealthAddition;
        [Range(0,1)] public float speedPercentIncrease;
        [Range(0,1)] public float jumpHeightIncrease;
        public float fireRateAddition;
        public int damageAddition;
        public float accuracyIncrease;

    }

    [System.Serializable]
    struct ItemDrop{
        public Item item;
        [Range(0,1)]
        public float change;
    }

    [CreateAssetMenu(fileName = "All Items", menuName = "Shooter/Player/All Items")]
    public class AllItems : ScriptableObject{
        public Item[] allItems;
        public Item SelectRandomItem(){
            return allItems[Random.Range(0, allItems.Length)];
        }
    }

}