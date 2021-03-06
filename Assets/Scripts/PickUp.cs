﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public enum TypePickup
    {
        HP_REPLENISH,
        COIN,
        BOMB,
        OTHER
    }

    public TypePickup type;
    public int amount = 1;
    public Item item;

    public void SetItem(Item i)
    {
        item = i;
        GetComponent<SpriteRenderer>().sprite = item.Sprite;
        Debug.Log("change sprite " + item.Sprite.name);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            switch (type)
            {
                case TypePickup.HP_REPLENISH:
                    col.SendMessage("SetReplenishHP", amount);
                    AudioManager.instance.Play("Heart");
                    break;
                case TypePickup.COIN:
                    col.SendMessage("SetAddCoin", amount);
                    AudioManager.instance.Play("Coin");
                    break;
                case TypePickup.BOMB:
                    /*List<object> temp = new List<object>
                    {
                        TypePickup.BOMB,
                        amount
                    };
                    col.SendMessage("AddToInventory", temp);
                    */

                    Inventory.Instance.AddItem("bomb", amount);
                    break;
                case TypePickup.OTHER:
                    Inventory.Instance.AddItem(item.ID);
                    break;
                default:
                    break;
            }
            
            Destroy(gameObject);
        }
    }
}
