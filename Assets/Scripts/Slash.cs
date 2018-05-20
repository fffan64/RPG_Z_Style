using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour {

    [Tooltip("Wait X seconds before destroy the object")]
    public float waitBeforeDestroy;

    [HideInInspector]
    public Vector2 mov;

    public float speed;
	public int damage = 1;

    private void Awake()
    {
        FindObjectOfType<AudioManager>().Play("Sword_Magic");
    }

    // Update is called once per frame
    void Update () {
        transform.position += new Vector3(mov.x, mov.y, 0f) * speed * Time.deltaTime;
	}

    /*
    private IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Object")
        {
            yield return new WaitForSeconds(waitBeforeDestroy);
            Destroy(gameObject);
        } else if(col.tag != "Player" && col.tag != "Attack")
        {
            if(col.tag == "Enemy")
            {
                col.SendMessage("Attacked");
            }
            Destroy(gameObject);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Object")
        {
            StartCoroutine(DoDestroy());
        }
        else if (col.tag != "Player" && col.tag != "Attack")
        {
            if (col.tag == "Enemy")
            {
                col.SendMessage("TakeDamage", damage);
            }
            FindObjectOfType<AudioManager>().Stop("Sword_Magic");
            Destroy(gameObject);
        }
    }

    IEnumerator DoDestroy()
    {
        yield return new WaitForSeconds(waitBeforeDestroy);
        FindObjectOfType<AudioManager>().Stop("Sword_Magic");
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        FindObjectOfType<AudioManager>().Stop("Sword_Magic");
    }
}
