﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour {

    public static Inventory Instance { get; set; }

    GameObject inventoryPanel;
    GameObject slotPanel;
    public GameObject inventorySlot;
    public GameObject equipSlot;
    public GameObject inventoryItem;

    int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    private bool show;

    public Item sword;
    public Item PotionLog;
    public PlayerWeaponController playerWeaponController;
    public ConsumableController consumableController;


    GameObject equipPanel;
    GameObject slotPanelEquip;

    int slotAmountEquip;
    public List<Item> itemsEquip = new List<Item>();
    //public List<GameObject> slotsEquip = new List<GameObject>();

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }

        slotAmount = 20;
        slotAmountEquip = 5;
        inventoryPanel = GameObject.Find("Inventory Panel");
        equipPanel = GameObject.Find("Equip Panel");
        slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;
        slotPanelEquip = equipPanel.transform.Find("Slot Panel Equip").gameObject;

        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
            slots[i].transform.localScale = Vector3.one;
        }

        /*
        for (int i = slotAmount; i < slotAmount + slotAmountEquip; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanelEquip.transform);
            slots[i].transform.localScale = Vector3.one;
        }*/
        for (int i = slotAmount; i < slotAmount + slotAmountEquip; i++)
        {
            itemsEquip.Add(new Item());
            slots.Add(Instantiate(equipSlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanelEquip.transform);
            slots[i].transform.localScale = Vector3.one;
        }


        AddItem(0);
        AddItem(0);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(1);
        AddItem(2);
        AddItem(2);
        AddItem(2);


        //playerWeaponController = GetComponent<PlayerWeaponController>();
        //consumableController = GetComponent<ConsumableController>();

        /*
        List<BaseStat> swordStats = new List<BaseStat>();
        swordStats.Add(new BaseStat(6, "Power", "Your power level."));
        //sword = new Item(10,Item.Type.weapon,"Sword","A sword", 10, 10, 10, 10, true, 0, "sword_ordinary", swordStats);
        sword = new Item(10, Item.Type.weapon, "Sword", "A sword", 10, 10, 10, 10, true, 0, "staff_fire", swordStats);

        PotionLog = new Item(11, Item.Type.consumable, "Potion Log", "Drink this to log something cool!", 0, 0, 0, 0, true, 0, "potion_log", new List<BaseStat>(), "Drink", false);
        */
        AddItem("sword_ordinary");
        AddItem("staff_fire");
        AddItem("potion_log");

        ShowInventory(show);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            show = !show;
            if(show)
            {
                AudioManager.instance.Play("Inventory_Open");
            } else
            {
                AudioManager.instance.Play("Inventory_Close");
            }
            ShowInventory(show);
        }
    }

    private void ShowInventory(bool show)
    {
        inventoryPanel.SetActive(show);
        equipPanel.SetActive(show);
    }

    public void AddItemToEquipSlot(string slug)
    {
        Item itemToAdd = ItemDatabase.Instance.FetchItemBySlug(slug);
        AudioManager.instance.Play("PickUp");
        
        for (int i = 0; i < itemsEquip.Count; i++)
        {
            if (itemsEquip[i].ID == -1)
            {
            itemsEquip[i] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem);
                ItemData iData = itemObj.GetComponent<ItemData>();
                iData.item = itemToAdd;
                iData.slot = slotAmount + i;
                iData.amount = 1;
                itemObj.transform.SetParent(slots[slotAmount + i].transform);
                //itemObj.transform.SetParent(slotsEquip[i].transform);
                itemObj.transform.localScale = Vector3.one;
                itemObj.transform.localPosition = Vector3.zero;
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                itemObj.name = itemToAdd.Title;
                break;
            }
        }
    }

    public void AddItem(string slug, int number)
    {
        Item itemToAdd = ItemDatabase.Instance.FetchItemBySlug(slug);
        for (int i = 0; i < number; i++)
        {
            DoAddItem(itemToAdd);
        }
    }

    public void AddItem(string slug)
    {
        Item itemToAdd = ItemDatabase.Instance.FetchItemBySlug(slug);
        DoAddItem(itemToAdd);
    }

    public void AddItem(int id)
    {
        Item itemToAdd = ItemDatabase.Instance.FetchItemByID(id);
        DoAddItem(itemToAdd);
    }

    public void DoAddItem(Item itemToAdd)
    {
        AudioManager.instance.Play("PickUp");
        if (itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == itemToAdd.ID)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == -1)
                {
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    ItemData iData = itemObj.GetComponent<ItemData>();
                    iData.item = itemToAdd;
                    iData.slot = i;
                    iData.amount = 1;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.localScale = Vector3.one;
                    itemObj.transform.localPosition = Vector3.zero;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Title;
                    break;
                }
            }
        }
        UIEventHandler.ItemAddedToInventory(itemToAdd);
    }

    public void RemoveItemFromEquipSlot(string slug)
    {
        Item itemToRemove = itemsEquip.FirstOrDefault(x => x.Slug == slug);
        if(itemToRemove != null)
        {
            int index = itemsEquip.IndexOf(itemToRemove);
            itemsEquip[index] = new Item();
            Destroy(slots[slotAmount + index].transform.GetChild(0).gameObject);
            //Destroy(slotsEquip[index].transform.GetChild(0).gameObject);
            Debug.Log("Remove item from Equipment");
        }
    }

    public void EquipItem(Item itemToEquip)
    {
        playerWeaponController.EquipWeapon(itemToEquip);
    }

    public void ConsumeItem(Item itemToConsume)
    {
        consumableController.ConsumeItem(itemToConsume);
    }

    public void Equip(Item item, int slot, bool deleteItem)
    {
        if (item.ItemType == Item.ItemTypes.armor || item.ItemType == Item.ItemTypes.weapon)
        {
            Debug.Log("EQUIP ITEM : " + item.Title);
            ItemData data = slots[slot].transform.GetChild(0).GetComponent<ItemData>();
            data.amount--;
            data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();

            Debug.Log("Equipped => do stuff");
            Inventory.Instance.EquipItem(item);

            if (deleteItem)
            {
                items[slot] = new Item();
                Destroy(slots[slot].transform.GetChild(0).gameObject);
                Debug.Log("Remove item from inventory");
            }
        }
        else
        {
            Debug.LogWarning("This item is not a equipable item ! : " + item.ItemType.ToString());
        }
    }

    public void UseConsumable(string slug)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Slug == slug)
            {
                if(Inventory.Instance.slots[i].transform.GetChild(0).GetComponent<ItemData>().amount > 1)
                {
                    UseConsumable(ItemDatabase.Instance.FetchItemBySlug(slug), i, false);
                }
                else
                {
                    UseConsumable(ItemDatabase.Instance.FetchItemBySlug(slug), i, true);
                }
                break;
            }
        }
    }

    public void UseConsumable(Item item, int slot, bool deleteItem)
    {
        if(item.ItemType == Item.ItemTypes.consumable)
        {
            Debug.Log("USED CONSUMABLE : " + item.Title);
            ItemData data = slots[slot].transform.GetChild(0).GetComponent<ItemData>();
            data.amount--;
            data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();

            Debug.Log("Consumed => do stuff");
            Inventory.Instance.ConsumeItem(item);

            if (deleteItem)
            {
                items[slot] = new Item();
                Destroy(slots[slot].transform.GetChild(0).gameObject);
                Debug.Log("Remove item from inventory");
            }
        } else
        {
            Debug.LogWarning("This item is not a consumable ! : " + item.ItemType.ToString());
        }
    }






    public bool CheckIfItemIsInInventory(Item item)
    {
        if(items.FirstOrDefault(x => x.ID == item.ID) != null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public bool CheckIfItemWithSlugIsInInventory(string slug)
    {
        if (items.FirstOrDefault(x => x.Slug == slug) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     private void SaveInventory()
     {
         for(int i = 0; i < inventory.Count; i++)
         {
             PlayerPrefs.SetInt("Inventory " + i, inventory[i].itemID);
         }
     }

     private void LoadInventory()
     {
         for(int i = 0; i < inventory.Count; i++)
         {
             inventory[i] = PlayerPrefs.GetInt("Inventory " + i, -1) >= 0 ? database.items[PlayerPrefs.GetInt("Inventory " + i)] : new Item();
         }
     }*/
}
