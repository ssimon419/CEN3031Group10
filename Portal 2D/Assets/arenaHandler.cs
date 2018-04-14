using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arenaHandler : MonoBehaviour {

	public GameObject[] platforms;
	public Sprite[] spr;

	private bool env=true;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	public void swap() {
		for (int i = 0; i < platforms.Length; ++i) {
			if (env) {
				platforms [i].GetComponent<SpriteRenderer> ().sprite = spr [0];
				platforms [i].tag = "environment";
				if (platforms [i].GetComponentInChildren<SimplePortal> ())
					platforms [i].GetComponentInChildren<SimplePortal> ().gameObject.SetActive (false);
			} else {
				platforms [i].GetComponent<SpriteRenderer> ().sprite = spr [1];
				platforms [i].tag = "ground";
			}
		}
		env = !env;
	}
}
