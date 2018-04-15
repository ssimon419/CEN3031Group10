using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss1_defense : MonoBehaviour {

	private Animator anim;

	private CircleCollider2D c_col;

	private bool absorb;

	// Use this for initialization
	void Awake() {
		anim = GetComponent<Animator> ();

	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("trail")) {
			anim.SetBool ("absorb", true);
			Destroy (other.gameObject);
		}
	}
	
	// Update is called once per frame

	void cancel_absorb(){
		Debug.Log ("caught trail");
		anim.SetBool ("absorb", false);
	}

	void cancel_def(){
		anim.SetBool ("defense", false);
	}

	void cancel_swap(){
		anim.SetBool ("swap", false);
	}
}
