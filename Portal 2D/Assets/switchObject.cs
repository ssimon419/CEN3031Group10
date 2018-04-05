using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchObject : MonoBehaviour {

	public GameObject[] objs;
	public Sprite[] sprites;

	private SpriteRenderer spr;
	private bool active=false;
	private bool in_area=false;

	void Awake(){
		spr = GetComponent<SpriteRenderer> ();
	}

	// Use this for initialization
	void Update(){
		if (Input.GetKeyDown("e") && in_area)
		{
			if (!active) {
				for (int i = 0; i < objs.Length; ++i) {
					objs [i].SetActive (true);
				}
				spr.sprite = sprites [1];
				active = true;
			} else {
				for (int i = 0; i < objs.Length; ++i) {
					objs [i].SetActive (false);
				}
				spr.sprite = sprites [0];
				active = false;
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			in_area = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			in_area = false;
		}
	}
}
