using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public enum TypePickup
    {
        HP_REPLENISH,
        COIN,
        BOMB
    }

    public TypePickup type;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            switch (type)
            {
                case TypePickup.HP_REPLENISH:
                    col.SendMessage("SetReplenishHP", amount);
                    FindObjectOfType<AudioManager>().Play("Heart");
                    break;
                case TypePickup.COIN:
                    col.SendMessage("SetAddCoin", amount);
                    FindObjectOfType<AudioManager>().Play("Coin");
                    break;
                case TypePickup.BOMB:
                    List<object> temp = new List<object>
                    {
                        TypePickup.BOMB,
                        amount
                    };
                    col.SendMessage("AddToInventory", temp);
                    FindObjectOfType<AudioManager>().Play("PickUp");
                    break;
                default:
                    break;
            }
            
            Destroy(gameObject);
        }
    }
}
