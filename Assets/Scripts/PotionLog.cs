﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionLog : MonoBehaviour, IConsumable {
    public void Consume()
    {
        AudioManager.instance.Play("Potion_Drink");
        Debug.Log("You drank a swig of the potion. Cool!");
        Destroy(gameObject);
    }

    public void Consume(CharacterStats stats)
    {
        AudioManager.instance.Play("Potion_Drink");
        Debug.Log("You drank a swig of the potion. Rad!");
        Destroy(gameObject);
    }
}
