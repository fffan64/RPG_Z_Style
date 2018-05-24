using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public Vector2 Direction { get; set; }
    public float Range { get; set; }
    public int Damage { get; set; }

    Vector2 spawnPosition;

    void Start () {
        Range = 20f;
        spawnPosition = transform.position;
        GetComponent<Rigidbody2D>().AddForce(Direction * 50f);
	}
	
	void Update () {
        if (Vector2.Distance(spawnPosition, transform.position) >= Range)
        {
            Extinguish();
        }
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Enemy")
        {
            col.transform.GetComponent<IEnemy>().TakeDamage(Damage);
            Extinguish();
        }        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        
        if(col.transform.tag == "Enemy")
        {
            col.transform.GetComponent<IEnemy>().TakeDamage(Damage);
        }
        Extinguish();
    }

    void Extinguish()
    {
        Destroy(gameObject);
    }
}
