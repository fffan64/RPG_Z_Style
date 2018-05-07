using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Warp : MonoBehaviour {

    public GameObject target;
    public GameObject targetMap;
    public bool disablePlayerHUD;

    bool start = false;
    bool isFadeIn = true;
    float alpha = 0f;
    float fadeTime = 1f;

    GameObject area;
    GameObject hudPlayer;

    private void Awake()
    {
        Assert.IsNotNull(target);
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        Assert.IsNotNull(targetMap);
        area = GameObject.FindGameObjectWithTag("Area");
        hudPlayer = GameObject.FindGameObjectWithTag("HUD_Player");
    }

    /*
    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Animator>().enabled = false;
            other.GetComponent<Player>().enabled = false;
            FadeIn();
            yield return new WaitForSeconds(fadeTime);
            other.transform.position = target.transform.GetChild(0).transform.position;
            Camera.main.GetComponent<MainCamera>().SetBound(targetMap);
            FadeOut();
            other.GetComponent<Animator>().enabled = true;
            other.GetComponent<Player>().enabled = true;
            StartCoroutine(area.GetComponent<Area>().ShowArea(targetMap.name));
        }
    }
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(DoWarp(other));
        }
    }

    private IEnumerator DoWarp(Collider2D other)
    {
        other.GetComponent<Animator>().enabled = false;
        other.GetComponent<Player>().enabled = false;
        FadeIn();
        yield return new WaitForSeconds(fadeTime);
        other.transform.position = target.transform.GetChild(0).transform.position;
        Camera.main.GetComponent<MainCamera>().SetBound(targetMap);
        FadeOut();
        other.GetComponent<Animator>().enabled = true;
        other.GetComponent<Player>().enabled = true;
        if (disablePlayerHUD)
        {
            hudPlayer.SendMessage("HideHUD");
        } else
        {
            hudPlayer.SendMessage("ShowHUD");
        }
        StartCoroutine(area.GetComponent<Area>().ShowArea(targetMap.name));
    }

    private void OnGUI()
    {
        if(!start)
        {
            return;
        }

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        Texture2D tex;
        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();

        GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), tex);

        if(isFadeIn)
        {
            alpha = Mathf.Lerp(alpha, 1.1f, fadeTime * Time.deltaTime);
        } else
        {
            alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);
            if(alpha < 0)
            {
                start = false;
            }
        }
    }

    void FadeIn()
    {
        start = true;
        isFadeIn = true;
    }

    void FadeOut()
    {
        isFadeIn = false;
    }
}
