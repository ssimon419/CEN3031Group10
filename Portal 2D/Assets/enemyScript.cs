using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour {
	public int health;
	public GameObject explosion;

	private SpriteRenderer spr;

	void Awake(){
		spr = GetComponent<SpriteRenderer> ();
	}

	void enemyDamage(int dmg){
		if (health > 0) {
			health-=dmg;
		} 
		if (health <= 0) {
			spr.enabled = false;
			if (explosion != null)
				explosion.SetActive (true);
			else
				Destroy (gameObject);
		}
	}
}
