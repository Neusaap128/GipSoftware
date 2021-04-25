
using UnityEngine;

namespace RiskOfRain
{

    public class ItemPickUp : Interactable
    {

        [SerializeField] Item item;

        public void SetItem(Item item){
            this.item = item;
        }

        protected override void OnCollision(PlayerStats player){
            player.AddItemToInventory(item);
            PickedUpItemText.SetPostionAndText(transform.position, "Picked up \n" + item.name);
            Destroy(gameObject);
        }
    }

}