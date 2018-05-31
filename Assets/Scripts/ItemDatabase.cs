using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using Newtonsoft.Json;

public class ItemDatabase : MonoBehaviour {
    public static ItemDatabase Instance { get; set; }
    public List<Item> Items { get; set; }

    public Sprite[] allSpritesIcons { get; set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        allSpritesIcons = Resources.LoadAll<Sprite>("Item Icons/");
        
        BuildDatabase();
    }

    public Item FetchItemByID(int id)
    {
        Item item = Items.FirstOrDefault(x => x.ID == id);
        if (item == null)
        {
            Debug.LogWarning("Item with id [" + id + "] not found !");
        }
        return item;
    }

    public Item FetchItemBySlug(string itemSlug)
    {
        Item item = Items.FirstOrDefault(x => x.Slug == itemSlug);
        if(item == null)
        {
            Debug.LogWarning("Item with slug [" + itemSlug + "] not found !");
        }
        return item;
    }


    void BuildDatabase()
    {
        Items = JsonConvert.DeserializeObject<List<Item>>(Resources.Load<TextAsset>("JSON/Items").ToString());
    }

}
