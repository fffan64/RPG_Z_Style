using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour {

    public float waitBeforePlay;

    Animator anim;
    Coroutine manager;
    bool loaded;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}

    public void AuraStart()
    {
        AudioManager.instance.Play("Sword_Charging");
        manager = StartCoroutine(Manager());
        anim.Play("Aura_Idle");
    }

    public void AuraStop()
    {
        AudioManager.instance.Stop("Sword_Charging");
        StopCoroutine(manager);
        anim.Play("Aura_Idle");
        loaded = false;
    }

    public IEnumerator Manager()
    {
        yield return new WaitForSeconds(waitBeforePlay);
        anim.Play("Aura_Play");
        loaded = true;
        AudioManager.instance.Play("Sword_Magic_Ready");
    }

    public bool IsLoaded()
    {
        return loaded;
    }
}
