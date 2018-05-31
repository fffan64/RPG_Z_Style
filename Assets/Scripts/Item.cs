using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public class Item {

    public enum ItemTypes
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
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public ItemTypes ItemType { get; set; }
    public string Special { get; set; }
    public string Effect { get; set; }

    public List<BaseStat> Stats { get; set; }
    public string ActionName { get; set; }
    public bool ItemModifier { get; set; }

    public Item(int id, ItemTypes type, string title, string description, int value, int power, int defence, int vitality, bool stackable, int rarity, string slug, List<BaseStat> _Stats)
    {

        ID = id;
        ItemType = type;
        Title = title;
        Value = value;
        Power = power;
        Defence = defence;
        Vitality = vitality;
        Description = description;
        Stackable = stackable;
        Rarity = rarity;
        Slug = slug;
        Sprite = ItemDatabase.Instance.allSpritesIcons.Where(x => x.name == slug).SingleOrDefault();
        Special = "Nothing special...";
        Effect = "No effect...";
        Stats = _Stats;
    }

    [JsonConstructor]
    public Item(int id, ItemTypes type, string title, string description, int value, int power, int defence, int vitality, bool stackable, int rarity, string slug, List<BaseStat> _Stats, string _ActionName, bool _ItemModifier)
    {
        
        ID = id;
        ItemType = type;
        Title = title;
        Value = value;
        Power = power;
        Defence = defence;
        Vitality = vitality;
        Description = description;
        Stackable = stackable;
        Rarity = rarity;
        Slug = slug;
        Sprite = ItemDatabase.Instance.allSpritesIcons.Where(x => x.name == slug).SingleOrDefault();
        Special = "Nothing special...";
        Effect = "No effect...";

        Stats = _Stats;
        ActionName = _ActionName;
        ItemModifier = _ItemModifier;
    }

    public Item()
    {
        ID = -1;
    }
}
