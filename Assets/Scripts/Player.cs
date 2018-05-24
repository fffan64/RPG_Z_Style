using System;
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
    [HideInInspector]
    public Vector2 mov;
    [HideInInspector]
    public bool movePrevent;

    public int maxHp = 12;
    public int hp;
    public int xp;
    public int level = 1;
    public int coin;

    public int[] levelUpXpNeeded;

    GameObject hudPlayer;

    private bool xpChanged;
    private bool hpChanged;
    private bool coinChanged;

    private bool blockAll;

    public GameObject bomb;
    public float distanceFromPlayerSpawn = 1f;
    
    [HideInInspector]
    public Vector2 prevMov;

    public CharacterStats characterStats;

    private void Awake()
    {
        Assert.IsNotNull(initialMap);
        Assert.IsNotNull(slashPrefab);
    }

    // Use this for initialization
    void Start () {

        CombatEvents.OnEnemyDeath += EnemyToExperience;
        Quest.OnGiveQuestExperience += GetQuestExperience;

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        Camera.main.GetComponent<MainCamera>().SetBound(initialMap);

        hp = maxHp;
        hudPlayer = GameObject.FindGameObjectWithTag("HUD_Player");

        characterStats = new CharacterStats(10, 10, 10);

        hudPlayer.GetComponent<HUD>().SetLevel(level);
        hudPlayer.GetComponent<HUD>().SetXp(xp, levelUpXpNeeded[level]);
        hudPlayer.GetComponent<HUD>().SetHp(hp, maxHp);
        hudPlayer.GetComponent<HUD>().SetCoin(coin);
    }

    private void GetQuestExperience(Quest quest)
    {
        xp += quest.ExperienceRewarded;
        xpChanged = true;
    }

    // Update is called once per frame
    void Update () {

        if (!blockAll && !FindObjectOfType<GameManager>().gameIsPaused)
        {
            Movements();

            Animations();
            
            BombAttack();

            PreventMovement();

            UpdateHUD();
        }
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
            prevMov = mov;
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    public void PlayFootStepLeft()
    {
        FindObjectOfType<AudioManager>().Play("Footstep1");
    }
    public void PlayFootStepRight()
    {
        FindObjectOfType<AudioManager>().Play("Footstep2");
    }

    void BombAttack()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Instantiate(bomb, transform.position + (new Vector3(prevMov.x, prevMov.y, 0)).normalized * distanceFromPlayerSpawn, Quaternion.identity);
        }
    }
    
    void PreventMovement()
    {
        if (movePrevent)
        {
            mov = Vector2.zero;
        }
    }

    public IEnumerator EnableMovementAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        movePrevent = false;
    }
    
    public void EnemyToExperience(IEnemy enemy)
    {   
        xp += enemy.Experience;
        xpChanged = true;
    }

    void UpdateHUD()
    {
        if(hpChanged)
        {
            hudPlayer.GetComponent<HUD>().SetHp(hp, maxHp);
            hpChanged = false;
        }
        if(xp >= levelUpXpNeeded[level])
        {
            level++;
            FindObjectOfType<AudioManager>().Play("UI_LevelUp");
            hudPlayer.GetComponent<Animator>().SetTrigger("levelup");
            hudPlayer.GetComponent<HUD>().SetLevel(level);
        }
        if(xpChanged)
        {
            hudPlayer.GetComponent<HUD>().SetXp(xp, levelUpXpNeeded[level]);
            xpChanged = false;
        }
        if (coinChanged)
        {
            hudPlayer.GetComponent<HUD>().SetCoin(coin);
            coinChanged = false;
        }
    }

    public void Attacked(int damage)
    {
        hpChanged = true;
        hp -= damage;
        FindObjectOfType<AudioManager>().Play("Player_Hurt");
        GetComponent<BlinkingFX>().startBlinking = true;
        if (hp <= 0)
        {
            //Destroy(gameObject);
            //gameObject.SetActive(false);
            Debug.Log("DEAD !");
            StartCoroutine(DoDeath());
        }
        UIEventHandler.HealthChanged(hp, maxHp);
    }

    IEnumerator DoDeath()
    {
        yield return new WaitForSeconds(5f);
    }

    public void SetReplenishHP(int amount)
    {
        hp++;
        hp = Mathf.Clamp(hp, 0, maxHp);
        hpChanged = true;
    }

    public void SetAddCoin(int amount)
    {
        coin+=amount;
        coinChanged = true;
    }

    public void AddToInventory(List<object> list)
    {
        PickUp.TypePickup type = (PickUp.TypePickup)list[0];
        int amount = (int)list[1];
        Debug.Log("Adding " + amount + " x " + type.ToString() + " to inventory !");
        switch (type)
        {
            case PickUp.TypePickup.HP_REPLENISH:
                break;
            
            default:
                break;
        }
    }

    public void BlockAllUpdate()
    {
        blockAll = true;
    }
    public void ReleaseAllUpdate()
    {
        blockAll = false;
    }
}
