using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    Image HUD_LVL_1;
    Image HUD_LVL_2;
    Image HUD_XP_BAR;

    List<Image> hearts = new List<Image>();

    public Sprite[] allNumbers;
    public Sprite[] allHearts;

    private void Start()
    {
        HUD_LVL_1 = GameObject.FindGameObjectWithTag("HUD_LVL_1").GetComponent<Image>();
        HUD_LVL_2 = GameObject.FindGameObjectWithTag("HUD_LVL_2").GetComponent<Image>();
        HUD_XP_BAR = GameObject.FindGameObjectWithTag("HUD_XP_BAR").GetComponent<Image>();
        hearts.Add(GameObject.FindGameObjectWithTag("HUD_HP_1").GetComponent<Image>());
        hearts.Add(GameObject.FindGameObjectWithTag("HUD_HP_2").GetComponent<Image>());
        hearts.Add(GameObject.FindGameObjectWithTag("HUD_HP_3").GetComponent<Image>());
    }

    public void SetLevel(int level)
    {
        int decade = level/10;
        int unit = level % 10;

        if(allNumbers.Length == 10)
        {
            HUD_LVL_1.sprite = allNumbers[decade];
            HUD_LVL_2.sprite = allNumbers[unit];
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
            nbQuartHeartLeft = (int)(4 * (div * nbHearts - (int)(div * nbHearts)));
        }
        Debug.Log("Hearts = " + nbHeartsToFill + " / Quarter heart more = " + nbQuartHeartLeft);

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
