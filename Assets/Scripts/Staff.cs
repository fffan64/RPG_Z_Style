using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon, IProjectileWeapon
{
    private GameObject playerGameObject;
    private Player playerScript;
    private Animator animator;
    private Aura aura;
    private CircleCollider2D attackCollider;

    public List<BaseStat> Stats { get; set; }
    public int CurrentDamage { get; set; }
    public Transform ProjectileSpawn { get; set; }

    Fireball fireball;

    public GameObject slashPrefab;

    private bool triggeredNormalAttack = false;
    //private bool triggeredSpecialAttack = false;

    private void Start()
    {
        fireball = Resources.Load<Fireball>("Weapons/Projectiles/Fireball");
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerScript = FindObjectOfType<Player>();
        animator = playerGameObject.GetComponent<Animator>();
        attackCollider = GetComponent<CircleCollider2D>();
        aura = playerGameObject.transform.GetChild(1).GetComponent<Aura>();
        attackCollider.enabled = false;
        //default pos at startup
        transform.localPosition = new Vector2(playerScript.prevMov.x / 2, playerScript.prevMov.y / 2);
        Vector3 v = playerScript.prevMov.normalized;
        transform.localScale = new Vector2(v.x <= 0 ? 1 : -1, v.y >= 0 ? 1 : -1);
    }

    private void Update()
    {
        NormalAttack();
        SlashAttack();
    }

    void SlashAttack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool loading = stateInfo.IsName("Player_Slash");

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            animator.SetTrigger("loading");
            aura.AuraStart();
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            animator.SetTrigger("attacking");
            if (aura.IsLoaded())
            {
                float angle = Mathf.Atan2(animator.GetFloat("movY"), animator.GetFloat("movX")) * Mathf.Rad2Deg;
                GameObject slashObj = Instantiate(slashPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
                Slash slash = slashObj.GetComponent<Slash>();
                slash.mov.x = animator.GetFloat("movX");
                slash.mov.y = animator.GetFloat("movY");
            }
            aura.AuraStop();
            StartCoroutine(playerScript.EnableMovementAfter(0.4f));
        }

        if (loading)
        {
            playerScript.movePrevent = true;
        }
    }

    void NormalAttack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool attacking = stateInfo.IsName("Player_attack");

        if (triggeredNormalAttack && !attacking)
        {
            animator.SetTrigger("attacking");
            AudioManager.instance.Play("Staff_Fire_Attack");
        }

        if (playerScript.mov != Vector2.zero)
        {
            transform.localPosition = new Vector2(playerScript.mov.x / 2, playerScript.mov.y / 2);
            Vector3 v = playerScript.mov.normalized;
            transform.localScale = new Vector2(v.x <= 0 ? 1 : -1, v.y >= 0 ? 1 : -1);
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
        triggeredNormalAttack = false;
    }

    public void PerformAttack(int damage)
    {
        CurrentDamage = damage;
        Debug.Log("Staff Attack !");
        triggeredNormalAttack = true;
        CastProjectile();
    }

    public void PerformSpecialAttack()
    {
        Debug.Log("Staff Special Attack !");
        //triggeredSpecialAttack = true;
    }

    public void CastProjectile()
    {
        Fireball fireballInstance = Instantiate(fireball, ProjectileSpawn.position, ProjectileSpawn.rotation);
        fireballInstance.Damage = CurrentDamage;
        if(playerScript.mov == Vector2.zero)
            fireballInstance.Direction = playerScript.prevMov;
        else
            fireballInstance.Direction = playerScript.mov;
    }
}
