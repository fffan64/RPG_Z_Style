using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPower : MonoBehaviour, IConsumable
{


    public void Consume()
    {
        AudioManager.instance.Play("Potion_Drink");
        Debug.Log("You drank a swig of the potion. POWERRRR!");
        Destroy(gameObject);
    }

    public void Consume(CharacterStats stats)
    {
        AudioManager.instance.Play("Potion_Drink");
        Debug.Log("You drank a swig of the potion. POWERRRR!");
        Destroy(gameObject);
    }
}
