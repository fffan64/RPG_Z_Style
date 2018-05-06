using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public enum TypePickup
    {
        HP_REPLENISH,
        COIN
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
                    break;
                case TypePickup.COIN:
                    col.SendMessage("SetAddCoin", amount);
                    break;
                default:
                    break;
            }
            
            Destroy(gameObject);
        }
    }
}
