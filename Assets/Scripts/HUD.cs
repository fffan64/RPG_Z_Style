using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    GameObject HUDPLayer;

    Image HUD_XP_BAR;
    List<Image> levels = new List<Image>();
    List<Image> hearts = new List<Image>();
    List<Image> coins = new List<Image>();

    public bool hideHUD;
    public Sprite[] allNumbers;
    public Sprite[] allHearts;


    private void Awake()
    {
        HUDPLayer = GameObject.FindGameObjectWithTag("HUD_Player");
        HUD_XP_BAR = HUDPLayer.transform.GetChild(0).GetChild(2).GetComponent<Image>();
        //Get all hearts images
        for(int i = 0; i < HUDPLayer.transform.GetChild(0).GetChild(0).childCount; i++)
        {
            hearts.Add(HUDPLayer.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Image>());
        }
        //Get all level images
        for (int i = 0; i < HUDPLayer.transform.GetChild(0).GetChild(1).childCount; i++)
        {
            levels.Add(HUDPLayer.transform.GetChild(0).GetChild(1).GetChild(i).GetComponent<Image>());
        }
        //Get all coins images
        for (int i = 0; i < HUDPLayer.transform.GetChild(1).childCount; i++)
        {
            coins.Add(HUDPLayer.transform.GetChild(1).GetChild(i).GetComponent<Image>());
        }
    }

    public void ShowHUD()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void HideHUD()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void SetLevel(int level)
    {
        int decade = level/10;
        int unit = level % 10;

        if(allNumbers.Length == 10)
        {
            levels[0].sprite = allNumbers[decade];
            levels[1].sprite = allNumbers[unit];
        } else
        {
            Debug.LogError("Look like all sprites number for level are not set properly (0 to 9 sprites)");
        }
    }

    public void SetXp(int xp, int xpForLevelUp)
    {
        float amount = (float)xp / xpForLevelUp;
        HUD_XP_BAR.fillAmount = Mathf.Clamp(amount, 0f, 1f);
    }

    public void SetCoin(int coin)
    {
        int hundred = coin / 100 % 10;
        int decade = coin / 10 % 10;
        int unit = coin % 10;

        if (allNumbers.Length == 10)
        {
            coins[0].sprite = allNumbers[hundred];
            coins[1].sprite = allNumbers[decade];
            coins[2].sprite = allNumbers[unit];
        }
        else
        {
            Debug.LogError("Look like all sprites number for level are not set properly (0 to 9 sprites)");
        }
    }

    public void SetHp(int hp, int maxHp)
    {
        //Debug.Log("HP = " + hp + " maxHP = " + maxHp);
        int nbHearts = 3;
        float div = (float)hp / maxHp;
        int nbHeartsToFill = (int)(div * nbHearts); // 
        int nbQuartHeartLeft = 0;
        if (nbHeartsToFill != nbHearts)
        {
            //4 quart of hearts = 1 heart
            nbQuartHeartLeft = Mathf.RoundToInt(4 * (div * nbHearts - (int)(div * nbHearts)));
        }
        //Debug.Log("Hearts = " + nbHeartsToFill + " / Quarter heart more = " + nbQuartHeartLeft);

        //All empty
        for (int j = 0; j < hearts.Count; j++)
        {
            hearts[j].sprite = allHearts[0];
        }
        
        int i = 0;
        for (i = 0; i < hearts.Count; i++)
        {
            if(i < nbHeartsToFill)
            {
                hearts[i].sprite = allHearts[4];
            }
            else
            {
                break;
            }
        }
        if (nbQuartHeartLeft > 0)
        {
            hearts[i].sprite = allHearts[nbQuartHeartLeft];
        }
    }
}
