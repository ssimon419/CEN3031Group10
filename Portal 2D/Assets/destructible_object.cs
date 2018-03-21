﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructible_object : MonoBehaviour {

	public int obj_health;
	public GameObject explosion;

	private SpriteRenderer spr;

	void Awake(){
		spr = GetComponent<SpriteRenderer> ();
	}

	void objectDamage(int damage){
		if (obj_health > 0) {
			obj_health-=damage;
		}
		if (obj_health <= 0) {
			spr.enabled = false;
			if (explosion != null)
				explosion.SetActive (true);
			else
				Destroy (gameObject);
		}
	}
}
