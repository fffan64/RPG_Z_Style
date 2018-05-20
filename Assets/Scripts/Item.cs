using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Item {

    public enum Type
    {
        weapon,
        armor,
        consumable,
        quest
    }

    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public int Power { get; set; }
    public int Defence { get; set; }
    public int Vitality { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }
    public Type TypeItem { get; set; }
    public string Special { get; set; }
    public string Effect { get; set; }

    private Sprite[] allSpritesIcons;

    public Item(int id, Type type, string title, string description, int value, int power, int defence, int vitality, bool stackable, int rarity, string slug)
    {
        if(allSpritesIcons == null)
        {
            allSpritesIcons = Resources.LoadAll<Sprite>("Item Icons/");
        }
        ID = id;
        TypeItem = type;
        Title = title;
        Value = value;
        Power = power;
        Defence = defence;
        Vitality = vitality;
        Description = description;
        Stackable = stackable;
        Rarity = rarity;
        Slug = slug;
        Sprite = allSpritesIcons.Where(x => x.name == slug).SingleOrDefault();
        Special = "Nothing special...";
        Effect = "No effect...";
    }

    public Item()
    {
        ID = -1;
    }
}
