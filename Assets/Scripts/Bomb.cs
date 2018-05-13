using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public float timer;
    public float areaOfEffect;
    public int damage;
    public LayerMask[] whatIsDestructible;
    public GameObject effect;
    private bool explosed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (explosed)
        {
            AnimatorStateInfo stateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Bomb_Explode") && stateInfo.normalizedTime >= 1)
            {
                Destroy(gameObject);
            }
        } else if (timer <= 0)
        {
            foreach(LayerMask layer in whatIsDestructible)
            {
                Collider2D[] objectsToDamage = Physics2D.OverlapCircleAll(transform.position, areaOfEffect, layer);
                for (int i = 0; i < objectsToDamage.Length; i++)
                {
                    objectsToDamage[i].SendMessage("Attacked", damage);
                }
            }
            
            if (effect)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
            }
            FindObjectOfType<AudioManager>().Play("Bomb");
            GetComponent<Animator>().Play("Bomb_Explode");
            explosed = true;
        } else
        {
            timer -= Time.deltaTime;
        }
	}

    /*
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player" && col.tag != "Attack")
        {
            if (col.tag == "Enemy")
            {
                col.SendMessage("Attacked");
            }
            FindObjectOfType<AudioManager>().Stop("Sword_Magic");
            Destroy(gameObject);
        }
    }
    */

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaOfEffect);
    }
}
