using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Linq;

public class ItemDatabase : MonoBehaviour {
    private List<Item> database = new List<Item>();
    private JsonData itemdData;

    void Start()
    {
        string sJSON = File.ReadAllText(Application.dataPath + "/StreamingAssets/items.json");
        itemdData = JsonMapper.ToObject(sJSON);
        ConstructItemDatabase();
        Debug.Log(FetchItemByID(1).Description);
    }

    public Item FetchItemByID(int id)
    {
        return database.FirstOrDefault(x => x.ID == id);
    }

    void ConstructItemDatabase()
    {
        for(int i = 0; i < itemdData.Count; i++)
        {
            database.Add(new Item((int)itemdData[i]["id"], itemdData[i]["title"].ToString(), itemdData[i]["description"].ToString(), (int)itemdData[i]["value"],
                (int)itemdData[i]["stats"]["power"], (int)itemdData[i]["stats"]["defence"], (int)itemdData[i]["stats"]["vitality"], (bool)itemdData[i]["stackable"], (int)itemdData[i]["rarity"], itemdData[i]["slug"].ToString()
                ));
        }
    }

    public List<Item> items = new List<Item>();
}
