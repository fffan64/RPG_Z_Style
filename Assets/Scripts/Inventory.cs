using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour {

    GameObject inventoryPanel;
    GameObject slotPanel;
    ItemDatabase database;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        database = GetComponent<ItemDatabase>();
        slotAmount = 20;
        inventoryPanel = GameObject.Find("Inventory Panel");
        slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;
        for(int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
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
    }

    public void AddItem(int id)
    {


        Item itemToAdd = database.FetchItemByID(id);

        if(itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd))
        {
            for(int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == id)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        } else
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
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Title;
                    break;
                }
            }
        }
    }

    bool CheckIfItemIsInInventory(Item item)
    {
        if(items.FirstOrDefault(x => x.ID == item.ID) != null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    /* public int slotsX, slotsY;
     public GUISkin skin;
     public List<Item> inventory = new List<Item>();
     public List<Item> slots = new List<Item>();
     public bool showInventory;
     private ItemDatabase database;
     private bool showTooltip;
     private string tooltip;

     private bool draggingItem;
     private Item draggedItem;
     private int prevIndex;

     // Use this for initialization
     void Start () {
         for (int i = 0; i < (slotsX * slotsY); i++)
         {
             slots.Add(new Item());
             inventory.Add(new Item());
         }
         database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
         AddItem(0);
         AddItem(1);
         AddItem(2);
     }

     private void Update()
     {
         if (Input.GetButtonDown("Inventory"))
         {
             showInventory = !showInventory;
         }
     }

     private void OnGUI()
     {
         if (GUI.Button(new Rect(40, 400, 100, 40), "Save"))
         {
             SaveInventory();
         }
         if (GUI.Button(new Rect(40, 450, 100, 40), "Load"))
         {
             LoadInventory();
         }
         tooltip = "";
         GUI.skin = skin;
         if (showInventory)
         {
             DrawInventory();
             if (showTooltip)
             {
                 GUI.Box(new Rect(Event.current.mousePosition.x + 15f, Event.current.mousePosition.y, 200, 200), tooltip, skin.GetStyle("Tooltip"));
             }
             if (draggingItem)
             {
                 GUI.DrawTexture(new Rect(Event.current.mousePosition.x + 15f, Event.current.mousePosition.y, 100, 100), draggedItem.itemIcon);
             }
         }
     }

     private void DrawInventory()
     {
         Event e = Event.current;
         int i = 0;
         for (int y = 0; y < slotsY; y++)
         {
             for (int x = 0; x < slotsX; x++)
             {
                 Rect slotRect = new Rect(x * 110, y * 110, 100, 100);
                 GUI.Box(slotRect, "", skin.GetStyle("Slot"));
                 slots[i] = inventory[i];
                 Item item = slots[i];
                 if (item.itemName != null)
                 {
                     GUI.DrawTexture(slotRect, item.itemIcon);
                     if (slotRect.Contains(e.mousePosition))
                     {
                         tooltip = CreateTooltip(item);
                         showTooltip = true;
                         if (e.button == 0 && e.type == EventType.MouseDrag && !draggingItem)
                         {
                             draggingItem = true;
                             prevIndex = i;
                             draggedItem = item;
                             inventory[i] = new Item();
                         }
                         if (e.type == EventType.MouseUp && draggingItem)
                         {
                             inventory[prevIndex] = inventory[i];
                             inventory[i] = draggedItem;
                             draggingItem = false;
                             draggedItem = null;
                         }
                         if(e.isMouse && e.type == EventType.MouseDown && e.button == 1)
                         {
                             if (item.itemType == Item.ItemType.Consumable)
                             {
                                 UseConsumable(item, i, true);
                             }
                         }
                     }
                 } else
                 {
                     if (slotRect.Contains(e.mousePosition))
                     {
                         if (e.type == EventType.MouseUp && draggingItem)
                         {
                             inventory[i] = draggedItem;
                             draggingItem = false;
                             draggedItem = null;
                         }
                     }
                 }
                 if(tooltip == "")
                 {
                     showTooltip = false;
                 }
                 i++;
             }
         }
     }

     private string CreateTooltip(Item item)
     {
         tooltip = "<color=#4DA4BF>" + item.itemName + "</color>\n\n" + "<color=#f2f2f2>" + item.itemDesc + "</color>\n\n";
         return tooltip;
     }

     private void RemoveItem(int id)
     {
         for (int i = 0; i < inventory.Count; i++)
         {
             if (inventory[i].itemID == id)
             {
                 inventory[i] = new Item();
                 break;
             }
         }
     }

     private void AddItem(int id)
     {
         for (int i = 0; i < inventory.Count; i++)
         {
             if(inventory[i].itemName == null)
             {
                 for (int j = 0; j < database.items.Count; j++)
                 {
                     if(database.items[j].itemID == id)
                     {
                         inventory[i] = database.items[j];
                     }
                 }
                 break;
             }
         }
     }

     private void UseConsumable(Item item, int slot, bool deleteItem)
     {
         switch (item.itemID)
         {
             case 2:
                 Debug.Log("USED CONSUMABLE : " + item.itemName);
                 break;
             default:
                 break;
         }

         if (deleteItem)
             inventory[slot] = new Item();
     }

     private bool InventoryContains(int id)
     {
         for(int i = 0; i < inventory.Count; i++)
         {
             if (inventory[i].itemID == id)
                 return true;
         }
         return false;
     }

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
