﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableController : MonoBehaviour {

    CharacterStats stats;

	void Start () {
        stats = GetComponent<Player>().characterStats;
	}

    public void ConsumeItem(Item item)
    {
        GameObject itemToSpawn = Instantiate(Resources.Load<GameObject>("Consumables/" + item.Slug));
        if (item.ItemModifier)
        {
            if(item.Stats != null)
            {
                stats.AddStatBonus(item.Stats);
            }
            itemToSpawn.GetComponent<IConsumable>().Consume(stats);
        } else
        {
            itemToSpawn.GetComponent<IConsumable>().Consume();
        }
    }
}
