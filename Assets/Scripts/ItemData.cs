using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {

    public Item item;
    public int amount;
    public int slot;

    private Inventory inv;
    private Tooltip tooltip;
    private Vector2 offset;

    private void Start()
    {
        inv = Inventory.Instance;//GameObject.Find("Inventory").GetComponent<Inventory>();
        tooltip = inv.GetComponent<Tooltip>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item != null)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                AudioManager.instance.Play("Inventory_DragStart");
                offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
                transform.SetParent(transform.parent.parent);
                transform.position = eventData.position - offset;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            } else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (item.ItemType == Item.ItemTypes.consumable)
                {
                    if (amount > 1)
                    {
                        inv.UseConsumable(item, slot, false);
                    }
                    else
                    {
                        inv.UseConsumable(item, slot, true);
                    }
                }
                else if (item.ItemType == Item.ItemTypes.armor || item.ItemType == Item.ItemTypes.weapon)
                {
                    if (amount > 1)
                    {
                        inv.Equip(item, slot, false);
                    }
                    else
                    {
                        inv.Equip(item, slot, true);
                    }
                }
                else
                {
                    Debug.Log("Item clicked is of type: " + item.ItemType.ToString());
                }
            }            
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item!= null)
        {
            transform.position = eventData.position - offset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        /*transform.SetParent(inv.slots[slot].transform);
        transform.position = inv.slots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        */
        OnEndDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(inv.slots[slot].transform);
        transform.position = inv.slots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        AudioManager.instance.Play("Inventory_DragStop");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }

    private void OnDestroy()
    {
        if(tooltip)
            tooltip.Deactivate();
    }
}
