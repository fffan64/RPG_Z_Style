using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour {

    public float speed = 4f;
    public GameObject initialMap;
    public GameObject slashPrefab;
    Animator anim;
    Rigidbody2D rb2d;
    Vector2 mov;
    bool movePrevent;

    CircleCollider2D attackCollider;

    Aura aura;

    private void Awake()
    {
        Assert.IsNotNull(initialMap);
        Assert.IsNotNull(slashPrefab);
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        attackCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        attackCollider.enabled = false;

        Camera.main.GetComponent<MainCamera>().SetBound(initialMap);

        aura = transform.GetChild(1).GetComponent<Aura>();
    }

    // Update is called once per frame
    void Update () {

        Movements();

        Animations();

        SwordAttack();

        SlashAttack();

        PreventMovement();
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + mov * speed * Time.deltaTime);
    }

    void Movements()
    {
        mov = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
    }

    void Animations()
    {
        if (mov != Vector2.zero)
        {
            anim.SetFloat("movX", mov.x);
            anim.SetFloat("movY", mov.y);
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    void SwordAttack()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool attacking = stateInfo.IsName("Player_attack");

        if (Input.GetKeyDown("space") && !attacking)
        {
            anim.SetTrigger("attacking");
        }

        if (mov != Vector2.zero)
        {
            attackCollider.offset = new Vector2(mov.x / 2, mov.y / 2);
        }

        if (attacking)
        {
            float playbackTime = stateInfo.normalizedTime;
            if (playbackTime > 0.33 && playbackTime < 0.66)
            {
                attackCollider.enabled = true;
            }
            else
            {
                attackCollider.enabled = false;
            }
        }
    }

    void SlashAttack()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool loading = stateInfo.IsName("Player_Slash");

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            anim.SetTrigger("loading");
            aura.AuraStart();
        } else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            anim.SetTrigger("attacking");
            if (aura.IsLoaded())
            {
                float angle = Mathf.Atan2(anim.GetFloat("movY"), anim.GetFloat("movX")) * Mathf.Rad2Deg;
                GameObject slashObj = Instantiate(slashPrefab, transform.position, Quaternion.AngleAxis(angle,   Vector3.forward));
                Slash slash = slashObj.GetComponent<Slash>();
                slash.mov.x = anim.GetFloat("movX");
                slash.mov.y = anim.GetFloat("movY");
            }
            aura.AuraStop();
            StartCoroutine(EnableMovementAfter(0.4f));
        }

        if (loading)
        {
            movePrevent = true;
        }
    }

    void PreventMovement()
    {
        if (movePrevent)
        {
            mov = Vector2.zero;
        }
    }

    IEnumerator EnableMovementAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        movePrevent = false;
    }
}
