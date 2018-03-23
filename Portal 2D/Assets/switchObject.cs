using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchObject : MonoBehaviour {

	public GameObject[] objs;

	private bool active=false;
	private bool in_area=false;
	// Use this for initialization
	void Update(){
		if (Input.GetKeyDown("e"))
		{
			Debug.Log ("hell borther");
			if (!active) {
				for (int i = 0; i < objs.Length; ++i) {
					objs [i].SetActive (true);
				}
				active = true;
			} else {
				for (int i = 0; i < objs.Length; ++i) {
					objs [i].SetActive (false);
				}
				active = false;
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			Debug.Log ("hello friends i am here\t");
			in_area = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			Debug.Log ("goodbye");
			in_area = false;
		}
	}
}
