using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal
{

    public string ItemSlug { get; set; }

    public CollectionGoal(Quest quest, string itemSlug, string description, bool completed, int currentAmount, int requiredAmount)
    {
        Quest = quest;
        ItemSlug = itemSlug;
        Description = description;
        Completed = completed;
        CurrentAmount = currentAmount;
        RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        if(Inventory.Instance.CheckIfItemWithSlugIsInInventory(ItemSlug))
        {
            //already done
            Debug.Log("Item: " + ItemSlug + " already in inventory ! Goal complete !");
            CurrentAmount++;
            Evaluate();
        } else
        {
            UIEventHandler.OnItemAddedToInventory += ItemPickedUp;
        }
    }

    void ItemPickedUp(Item item)
    {
        if (item.Slug == ItemSlug)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
