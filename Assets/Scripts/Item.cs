using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Item {

    public string itemName;
    public int itemID;
    public string itemDesc;
    public Texture2D itemIcon;
    public int itemPower;
    public int itemSpeed;
    public ItemType itemType;

    public enum ItemType
    {
        Weapon,
        Consumable,
        Quest
    }

    public Sprite[] allSpritesIcons;

    Texture2D ConvertSpriteToTexture(Sprite sprite)
    {
        try
        {
            //Debug.Log("sprite.rect.width != sprite.texture.width / " + sprite.rect.width + " : " + sprite.texture.width);
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D(Mathf.CeilToInt(sprite.textureRect.width), Mathf.CeilToInt(sprite.textureRect.height));
                //Color[] colors = newText.GetPixels();
                Color[] newColors = sprite.texture.GetPixels(Mathf.CeilToInt(sprite.textureRect.x),
                                                             Mathf.CeilToInt(sprite.textureRect.y),
                                                             Mathf.CeilToInt(sprite.textureRect.width),
                                                             Mathf.CeilToInt(sprite.textureRect.height));
                //Debug.Log(colors.Length + "_" + newColors.Length);
                newText.SetPixels(newColors);
                newText.filterMode = FilterMode.Point;
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
            return sprite.texture;
        }
    }

    public Item(string name, int id, string desc, int power, int speed, ItemType type)
    {
        if (allSpritesIcons == null)
        {
            allSpritesIcons = Resources.LoadAll<Sprite>("Item Icons/");
        }

        itemName = name;
        itemID = id;
        itemDesc = desc;
        Sprite s = allSpritesIcons.Where(x => x.name == name).SingleOrDefault();
        if(s)
        {
            itemIcon = ConvertSpriteToTexture(s);
        }
        itemPower = power;
        itemSpeed = speed;
        itemType = type;
    }

    public Item()
    {
        itemID = -1;
    }
}
