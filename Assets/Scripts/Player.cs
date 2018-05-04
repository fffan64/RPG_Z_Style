using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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

    public int maxHp = 12;
    public int hp;
    public int xp;
    public int level = 1;

    public int[] levelUpXpNeeded;

    GameObject hudTop;

    private bool xpChanged;
    private bool hpChanged;

    private void Awake()
    {
        Assert.IsNotNull(initialMap);
        Assert.IsNotNull(slashPrefab);

        hp = maxHp;
        hudTop = GameObject.FindGameObjectWithTag("HUD_Top");
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        attackCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        attackCollider.enabled = false;

        Camera.main.GetComponent<MainCamera>().SetBound(initialMap);

        aura = transform.GetChild(1).GetComponent<Aura>();

        hudTop.GetComponent<HUD>().SetLevel(level);
        hudTop.GetComponent<HUD>().SetXp(xp, levelUpXpNeeded[level]);
        hudTop.GetComponent<HUD>().SetHp(hp, maxHp);
    }

    // Update is called once per frame
    void Update () {

        Movements();

        Animations();

        SwordAttack();

        SlashAttack();

        PreventMovement();

        UpdateHUD();
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

            if (playbackTime > 0.33f && playbackTime < 0.66f)
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

    public void AddXp(int xpToAdd)
    {
        xp += xpToAdd;
        xpChanged = true;
    }

    void UpdateHUD()
    {
        if(hpChanged)
        {
            hudTop.GetComponent<HUD>().SetHp(hp, maxHp);
            hpChanged = false;
        }
        if(xp >= levelUpXpNeeded[level])
        {
            level++;
            //anim level up
            hudTop.GetComponent<HUD>().SetLevel(level);
        }
        if(xpChanged)
        {
            hudTop.GetComponent<HUD>().SetXp(xp, levelUpXpNeeded[level]);
            xpChanged = false;
        }
    }

    public void Attacked()
    {
        hpChanged = true;
        if (--hp <= 0)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
            StartCoroutine(DoDeath());
        }
    }

    IEnumerator DoDeath()
    {
        yield return new WaitForSeconds(5f);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
