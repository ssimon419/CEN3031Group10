using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arenaHandler : MonoBehaviour {

	public GameObject[] platforms;
	public Sprite[] spr;


	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	public void swap() {
		for (int i = 0; i < platforms.Length; ++i) {
			if (platforms[i].CompareTag("environment")) {
				platforms [i].GetComponent<SpriteRenderer> ().sprite = spr [1];
				platforms [i].tag = "ground";
			} else {
				platforms [i].GetComponent<SpriteRenderer> ().sprite = spr [0];
				platforms [i].tag = "environment";
				if (platforms [i].GetComponentInChildren<SimplePortal> ())
					platforms [i].GetComponentInChildren<SimplePortal> ().gameObject.SetActive (false);
			}
		}
	}

	public void swap(int a, int b) {
		for (int i = a; i < b+1; ++i) {
			if (platforms[i].CompareTag("environment")) {
				platforms [i].GetComponent<SpriteRenderer> ().sprite = spr [1];
				platforms [i].tag = "ground";
			} else {
				platforms [i].GetComponent<SpriteRenderer> ().sprite = spr [0];
				platforms [i].tag = "environment";
				if (platforms [i].GetComponentInChildren<SimplePortal> ())
					platforms [i].GetComponentInChildren<SimplePortal> ().gameObject.SetActive (false);
			}
		}
	}
}
