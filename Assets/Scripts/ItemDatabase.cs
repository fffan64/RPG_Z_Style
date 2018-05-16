using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {
    public List<Item> items = new List<Item>();

    private void Start()
    {
        items.Add(new Item("Amulet of Prayers", 0, "An amulet enchanted by prayers", 2, 0, Item.ItemType.Weapon));
        items.Add(new Item("White Shirt", 1, "A shirt that is white", 0, 0, Item.ItemType.Weapon));
        items.Add(new Item("Power Potion", 2, "A potion that temporary increase your power", 0, 0, Item.ItemType.Consumable));
    }
}
