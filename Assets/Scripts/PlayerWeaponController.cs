using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerWeaponController : MonoBehaviour {

    public GameObject playerHand;
    public GameObject EquippedWeapon { get; set; }

    Transform spawnProjectile;
    Item currentlyEquippedItem;
    IWeapon equippedWeapon;
    CharacterStats characterStats;

    private void Start()
    {
        spawnProjectile = transform.Find("ProjectileSpawn");
        characterStats = GetComponent<Player>().characterStats;
    }

    public void EquipWeapon(Item itemToEquip)
    {
        if(EquippedWeapon != null)
        {
            UnequipWeapon();
        }
        EquippedWeapon = (GameObject)Instantiate(Resources.Load<GameObject>("Weapons/" + itemToEquip.Slug), playerHand.transform.position, playerHand.transform.rotation);
        equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();
        if(EquippedWeapon.GetComponent<IProjectileWeapon>() != null)
            EquippedWeapon.GetComponent<IProjectileWeapon>().ProjectileSpawn = spawnProjectile;
        equippedWeapon.Stats = itemToEquip.Stats;
        currentlyEquippedItem = itemToEquip;
        EquippedWeapon.transform.SetParent(playerHand.transform);
        characterStats.AddStatBonus(itemToEquip.Stats);
        AudioManager.instance.Play("Equip_Weapon");
        UIEventHandler.ItemEquipped(itemToEquip);
        UIEventHandler.StatsChanged();

        Inventory.Instance.AddItemToEquipSlot(itemToEquip.Slug);
    }

    void UnequipWeapon()
    {
        Debug.Log("Adding Unequipped weapon back to inventory : " + currentlyEquippedItem.Slug);
        Inventory.Instance.AddItem(currentlyEquippedItem.Slug);
        characterStats.RemoveStatBonus(equippedWeapon.Stats);
        Destroy(EquippedWeapon.transform.gameObject);
        UIEventHandler.StatsChanged();

        Inventory.Instance.RemoveItemFromEquipSlot(currentlyEquippedItem.Slug);
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

    private int CalculateDamage()
    {
        int damageToDeal = (characterStats.GetStat(BaseStat.BaseStatType.Power).GetCalculatedStatValue() * 2)
            + Random.Range(2, 8);
        damageToDeal += CalculateCrit(damageToDeal);
        Debug.Log("Damage dealt: " + damageToDeal);
        return damageToDeal;
    }

    private int CalculateCrit(int damage)
    {
        if (Random.value <= .10f)
        {
            int critDamage = (int)(damage * Random.Range(.5f, .75f));
            return critDamage;
        }
        return 0;
    }

    public void PerformWeaponAttack()
    {
        if (EquippedWeapon != null)
        {
            equippedWeapon.PerformAttack(CalculateDamage());
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
