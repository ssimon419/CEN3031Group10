﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class area_boundary : MonoBehaviour {

	public Transform respawn;

	// Use this for initialization
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			coll.gameObject.GetComponentInChildren<playerScript> ().playerDamage (5);
			coll.gameObject.transform.position = respawn.position;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			other.GetComponentInChildren<playerScript> ().playerDamage (5);
			other.transform.position = respawn.position;
		}
	}
}
