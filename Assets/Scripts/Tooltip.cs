using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

    private Item item;
    private string data;
    private GameObject tooltip;

    private void Start()
    {
        tooltip = GameObject.Find("Tooltip");
        tooltip.SetActive(false);
    }

    private void Update()
    {
        if (tooltip.activeSelf)
        {
            tooltip.transform.position = Input.mousePosition; 
        }
    }

    public void Activate(Item item)
    {
        this.item = item;
        ConstructDataString();
        tooltip.SetActive(true);
    }

    public void Deactivate()
    {
        tooltip.SetActive(false);
    }

    public void ConstructDataString()
    {
        data = "<color=#215cba><b>" + item.Title + "</b></color>\n\n" + item.Description + "\n\n<color=#ba7720>";

        string sType = "";
        switch (item.ItemType)
        {
            case Item.ItemTypes.weapon:
                sType = "Weapon";
                data += "<i>Power: " + item.Power + "</i></color>";
                break;
            case Item.ItemTypes.armor:
                sType = "Armor";
                data += "<i>Defence: " + item.Defence + "</i></color>";
                break;
            case Item.ItemTypes.consumable:
                sType = "Consumable";
                data += "<i>Effect: " + item.Effect + "</i></color>";
                break;
            case Item.ItemTypes.quest:
                sType = "Quest";
                data += "<i>Spec. : " + item.Special + "</i></color>";
                break;
            default:
                break;
        }
        
        tooltip.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = data;
        tooltip.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = sType;

        Image img = tooltip.transform.GetChild(0).GetComponent<Image>();
        Color col = Color.white;
        switch (item.Rarity)
        {
            case 0:
                col = Color.white;
                break;
            case 1:
                ColorUtility.TryParseHtmlString("#d6e5ff", out col);
                break;
            case 2:
                ColorUtility.TryParseHtmlString("#dcffd6", out col);
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#d8d8d8", out col);
                break;
            case 4:
                ColorUtility.TryParseHtmlString("#ffd6d6", out col);
                break;
            case 5:
                ColorUtility.TryParseHtmlString("#fdffd6", out col);
                break;
            default:
                col = Color.white;
                break;
        }
        img.color = col;
    }
}
