using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour {

    public float scrollSpeed = 1f;

    Vector3 startPos;

    float newPos;

    float width;

    // Use this for initialization
    void Start () {
        startPos = transform.position;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        GameObject objectCopy = GameObject.Instantiate(this.gameObject, this.transform.position + new Vector3(width, 0, 0), this.transform.rotation, this.transform);
        objectCopy.transform.localScale = Vector3.one;
        Destroy(objectCopy.GetComponent<ScrollBackground>());
    }
	
	// Update is called once per frame
	void Update () {
        newPos = Mathf.Repeat(Time.time * -scrollSpeed, width);
        transform.position = startPos + Vector3.right * newPos - new Vector3(width, 0,0);
    }
}
