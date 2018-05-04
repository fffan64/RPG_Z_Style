using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    [Tooltip("Speed in World unit")]
    public float speed = 3f;

    GameObject player;
    Rigidbody2D rb2d;
    Vector3 target, dir;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();

        if(player != null)
        {
            target = player.transform.position;
            dir = (target - transform.position).normalized;
        }
    }

    private void FixedUpdate()
    {
        if(target != Vector3.zero)
        {
            rb2d.MovePosition(transform.position + (dir * speed) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "Player" || col.transform.tag == "Attack")
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }   
}
