using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Linq;
using System;

public class ItemDatabase : MonoBehaviour {
    public static ItemDatabase Instance { get; set; }
    private List<Item> database = new List<Item>();

    internal Item FetchItemBySlug(string itemSlug)
    {
        return database.FirstOrDefault(x => x.Slug == itemSlug);
    }

    private JsonData itemdData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        string sJSON = File.ReadAllText(Application.dataPath + "/StreamingAssets/items.json");
        itemdData = JsonMapper.ToObject(sJSON);
        ConstructItemDatabase();
    }

    public Item FetchItemByID(int id)
    {
        return database.FirstOrDefault(x => x.ID == id);
    }

    void ConstructItemDatabase()
    {
        for(int i = 0; i < itemdData.Count; i++)
        {
            database.Add(new Item((int)itemdData[i]["id"], (Item.Type)System.Enum.Parse(typeof(Item.Type), itemdData[i]["type"].ToString(), true), itemdData[i]["title"].ToString(), itemdData[i]["description"].ToString(), (int)itemdData[i]["value"],
                (int)itemdData[i]["stats"]["power"], (int)itemdData[i]["stats"]["defence"], (int)itemdData[i]["stats"]["vitality"], (bool)itemdData[i]["stackable"], (int)itemdData[i]["rarity"], itemdData[i]["slug"].ToString()
                ));
        }
    }

    public List<Item> items = new List<Item>();
}
