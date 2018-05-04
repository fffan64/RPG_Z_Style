using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Log : MonoBehaviour {

    public float visionRadius = 2.5f;
    public float attackRadius = 1f;
    public float speed = 1.5f;

    GameObject player;

    Vector3 initialPosition, target;

    Animator anim;
    Rigidbody2D rb2d;

    [Tooltip("Projectile prefab to instantiate")]
    public GameObject projPrefab;
    [Tooltip("Speed of attack (seconds between attacks)")]
    public float attackSpeed = 2f;
    bool attacking;

    [Tooltip("Health Points")]
    public int maxHp = 3;
    [Tooltip("Current Health Points")]
    public int hp;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        initialPosition = transform.position;

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        hp = maxHp;
    }

    private void Update()
    {
        target = initialPosition;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            player.transform.position - transform.position,
            visionRadius,
            1 << LayerMask.NameToLayer("Default")
        );

        Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
        Debug.DrawRay(transform.position, forward, Color.red);

        if(hit.collider != null)
        {
            if(hit.collider.tag == "Player")
            {
                target = player.transform.position;
            }
        }

        float distance = Vector3.Distance(target, transform.position);
        Vector3 dir = (target - transform.position).normalized;

        if(target != initialPosition && distance < attackRadius)
        {
            anim.SetFloat("movX", dir.x);
            anim.SetFloat("movY", dir.y);
            anim.Play("Enemy_Log_walk", -1, 0);

            if(!attacking)
            {
                StartCoroutine(Attack(attackSpeed));
            }
        } else
        {
            rb2d.MovePosition(transform.position + dir * speed * Time.deltaTime);

            anim.speed = 1;
            anim.SetFloat("movX", dir.x);
            anim.SetFloat("movY", dir.y);
            anim.SetBool("walking", true);
        }

        if(target == initialPosition && distance < 0.05f)
        {
            transform.position = initialPosition;
            anim.SetBool("walking", false);
        }

        Debug.DrawLine(transform.position, target, Color.green);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    IEnumerator Attack(float seconds)
    {
        attacking = true;
        if(target != initialPosition && projPrefab != null)
        {
            Instantiate(projPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(seconds);
        }
        attacking = false;
    }

    public void Attacked()
    {
        if(--hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnGUI()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);

        GUI.Box(new Rect(pos.x - 20, Screen.height - pos.y + 60, 40, 24), hp + "/" + maxHp);
    }
}
