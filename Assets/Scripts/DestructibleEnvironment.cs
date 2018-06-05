using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleEnvironment : MonoBehaviour {

    public int health = 1;
    public GameObject destroyEffect;
    public string destroyAnimName;
    public string destroySoundName;
    private bool destroyed;

    private void Update()
    {
        if (destroyed)
        {
            AnimatorStateInfo stateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(destroyAnimName) && stateInfo.normalizedTime >= 1)
            {
                Destroy(gameObject);
            }
        }
        else if (health <= 0)
        {
            if (destroyEffect)
            {
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
            }
            if(destroyAnimName != null || destroyAnimName != "")
            {
                GetComponent<Animator>().Play(destroyAnimName);
            }
            AudioManager.instance.Play(destroySoundName);
            destroyed = true;
        }
    }

    public void Attacked(int damage)
    {
        health -= damage;
    }
}
