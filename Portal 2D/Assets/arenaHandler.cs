using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arenaHandler : MonoBehaviour {

	public GameObject[] platforms;
	public Sprite[] spr;

	public bool is_boss_ship;

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

	void OnTriggerEnter2D(Collider2D other){
		if (is_boss_ship) {
			if (other.gameObject.CompareTag ("bullet")) {
				if (other.gameObject.GetComponent<bullet> ().friendly) {
					gameObject.GetComponent<enemyScript> ().enemyDamage (1);
				}
			} else if (other.gameObject.CompareTag ("pain")) {
				gameObject.GetComponent<enemyScript> ().enemyDamage (150);
			}
		}
	}
}
