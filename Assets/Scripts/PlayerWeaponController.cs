using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerWeaponController : MonoBehaviour {

    public GameObject playerHand;
    public GameObject EquippedWeapon { get; set; }

    IWeapon equippedWeapon;
    CharacterStats characterStats;

    private void Start()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    public void EquipWeapon(Item itemToEquip)
    {
        if(EquippedWeapon != null)
        {
            characterStats.RemoveStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
            Destroy(playerHand.transform.GetChild(0).gameObject);
        }
        EquippedWeapon = (GameObject)Instantiate(Resources.Load<GameObject>("Weapons/" + itemToEquip.Slug), playerHand.transform.position, playerHand.transform.rotation);
        equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();
        equippedWeapon.Stats = itemToEquip.Stats;
        EquippedWeapon.transform.SetParent(playerHand.transform);
        characterStats.AddStatBonus(itemToEquip.Stats);
        //Debug.Log(equippedWeapon.Stats[0].GetCalculatedStatValue());
        GameObject.FindGameObjectWithTag("HUD_Player").transform.Find("HUD_Top/WEAPON").GetComponent<Image>().sprite = ItemDatabase.Instance.allSpritesIcons.Where(x => x.name == itemToEquip.Slug).SingleOrDefault();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformWeaponAttack();
        } else if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            PerformWeaponAttackSpecial();
        }
    }

    public void PerformWeaponAttack()
    {
        if (EquippedWeapon != null)
        {
            equippedWeapon.PerformAttack();
        }
    }
    public void PerformWeaponAttackSpecial()
    {
        if (EquippedWeapon != null)
        {
            equippedWeapon.PerformSpecialAttack();
        }
    }
}
