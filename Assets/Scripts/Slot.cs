using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    public enum SlotType{
        Inventory,
        Equipment
    }

    public SlotType type;
    public int id;
    private Inventory inv;

    private void Start()
    {
        inv = Inventory.Instance;//GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
        if (type == SlotType.Inventory)
        {
            if (inv.items[id].ID == -1)
            {
                inv.items[droppedItem.slot] = new Item();
                inv.items[id] = droppedItem.item;
                droppedItem.slot = id;
            }
            else if (droppedItem.slot != id)
            {
                Transform item = this.transform.GetChild(0);
                item.GetComponent<ItemData>().slot = droppedItem.slot;
                item.transform.SetParent(inv.slots[droppedItem.slot].transform);
                item.transform.position = inv.slots[droppedItem.slot].transform.position;

                droppedItem.slot = id;
                droppedItem.transform.SetParent(transform);
                droppedItem.transform.position = transform.position;

                inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
                inv.items[id] = droppedItem.item;
            }
        }
        else
        {
            if (inv.itemsEquip[id].ID == -1)
            {
                inv.itemsEquip[droppedItem.slot] = new Item();
                inv.itemsEquip[id] = droppedItem.item;
                droppedItem.slot = id;
            }
            else if (droppedItem.slot != id)
            {
                Transform item = this.transform.GetChild(0);
                item.GetComponent<ItemData>().slot = droppedItem.slot;
                item.transform.SetParent(inv.slots[droppedItem.slot].transform);
                item.transform.position = inv.slots[droppedItem.slot].transform.position;
                //item.transform.SetParent(inv.slotsEquip[droppedItem.slot].transform);
                //item.transform.position = inv.slotsEquip[droppedItem.slot].transform.position;

                droppedItem.slot = id;
                droppedItem.transform.SetParent(transform);
                droppedItem.transform.position = transform.position;

                inv.itemsEquip[droppedItem.slot] = item.GetComponent<ItemData>().item;
                inv.itemsEquip[id] = droppedItem.item;
            }
        }
    }
}
