using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour {


    Animator anim;
    bool opened;
    bool playerInRange;
    Vector3 posPlayer;
    public GameObject loot;
    public string openingState;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !opened)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetTrigger("opening");
                GetComponents<BoxCollider2D>()[1].enabled = false;
            }
        }
    }

    public void SetOpened()
    {
        opened = true;
        anim.SetBool("open", true);

        Instantiate(loot, posPlayer, Quaternion.AngleAxis(0f, Vector3.zero));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" && !opened)
        {
            col.transform.GetChild(2).gameObject.SetActive(true);
            playerInRange = true;
            posPlayer = col.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.transform.GetChild(2).gameObject.SetActive(false);
            playerInRange = false;
        }
    }
}
