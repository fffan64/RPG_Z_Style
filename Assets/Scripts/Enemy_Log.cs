using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_Log : MonoBehaviour, IEnemy {

    public float visionRadius = 2.5f;
    public float attackRadius = 1f;
    public float speed = 1.5f;

    GameObject player;

    Vector3 initialPosition, target, localScale;

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
    [Tooltip("Experience Points given when defeated")]
    public int xp = 10;

    [Tooltip("Attack power (in HP)")]
    public int damage = 1;

    public string destroyState;
    public float timeForDisable = 0.25f;
    bool defeated;

    public int ID { get; set; }
    public int Experience { get; set; }

    public DropTable DropTable { get; set; }

    private void Start()
    {
        DropTable = new DropTable();
        DropTable.loot = new List<LootDrop>
        {
            new LootDrop("sword_ordinary", 25),
            new LootDrop("power_potion", 25),
            new LootDrop("white_shirt", 25)
        };
        ID = 0;
        Experience = xp;
        player = GameObject.FindGameObjectWithTag("Player");

        initialPosition = transform.position;

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        hp = maxHp;

        localScale = transform.GetChild(0).transform.localScale;

        enabled = false;
    }

    private void Update()
    {
        if (defeated)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(destroyState) && stateInfo.normalizedTime >= 1)
            {
                Destroy(gameObject);
            }
        }
        else
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

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    target = player.transform.position;
                }
            }

            float distance = Vector3.Distance(target, transform.position);
            Vector3 dir = (target - transform.position).normalized;

            if (target != initialPosition && distance < attackRadius)
            {
                anim.SetFloat("movX", dir.x);
                anim.SetFloat("movY", dir.y);
                anim.Play("Enemy_Log_walk", -1, 0);

                if (!attacking)
                {
                    PerformAttack();
                }
            }
            else
            {
                rb2d.MovePosition(transform.position + dir * speed * Time.deltaTime);

                anim.speed = 1;
                anim.SetFloat("movX", dir.x);
                anim.SetFloat("movY", dir.y);
                anim.SetBool("walking", true);
            }

            if (target == initialPosition && distance < 0.05f)
            {
                transform.position = initialPosition;
                anim.SetBool("walking", false);
            }

            Debug.DrawLine(transform.position, target, Color.green);            
        }
        //Update healthbar
        localScale.x = Mathf.Clamp((float)(hp) / maxHp, 0f, 1f);
        transform.GetChild(0).transform.localScale = localScale;
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
            Instantiate(projPrefab, transform.position, transform.rotation).SendMessage("SetDamage", damage);
            FindObjectOfType<AudioManager>().Play("Shot");
            yield return new WaitForSeconds(seconds);
        }
        attacking = false;
    }

    /*
    public void Attacked()
    {
        FindObjectOfType<AudioManager>().Play("Enemy_Log_Hurt");
        if (--hp <= 0)
        {
            StartCoroutine(DoDestroy());
        }
    }*/

    private IEnumerator DoDestroy()
    {
        Debug.Log("DESTROY ENEMY LOG!");
        defeated = true;
        anim.Play("Enemy_Log_Defeated");
        FindObjectOfType<AudioManager>().Play("Poof");


        // Pasados los segundos de espera desactivamos los colliders 2D
        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = false;
        }

        yield return new WaitForSeconds(timeForDisable);

        DropLoot();
        CombatEvents.EnemyDied(this);
    }

    private void DropLoot()
    {
        Item item = DropTable.GetDrop();
        Debug.Log("Dropped " + item);
        if (item != null)
        {
            Debug.Log("Dropped " + item.Title);
            GameObject instance = (GameObject)Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Pickup_Item.prefab", typeof(GameObject)), transform.position, Quaternion.identity);
            instance.GetComponent<PickUp>().SetItem(item);
        }
    }

    private void OnBecameInvisible()
    {
        //Debug.Log("Became invisible");
        enabled = false;
    }
    private void OnBecameVisible()
    {
        //Debug.Log("Became visible");
        enabled = true;
    }

    public void Die()
    {
        StartCoroutine(DoDestroy());
    }

    public void TakeDamage(int amount)
    {
        FindObjectOfType<AudioManager>().Play("Enemy_Log_Hurt");
        hp -= amount;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void PerformAttack()
    {
        StartCoroutine(Attack(attackSpeed));
    }
}
