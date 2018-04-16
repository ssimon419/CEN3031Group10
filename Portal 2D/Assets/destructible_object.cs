using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructible_object : MonoBehaviour {

	public int obj_health;
	public GameObject explosion;

	private SpriteRenderer spr;

	void Awake(){
		spr = GetComponent<SpriteRenderer> ();
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.CompareTag("Player")) {
			Debug.Log ("hit player!"+coll.relativeVelocity.magnitude);
			if (coll.relativeVelocity.magnitude > 25) {
				coll.gameObject.GetComponentInChildren<playerScript> ().playerDamage (10);
			}
		}
		if (coll.gameObject.CompareTag ("ground")) {
			if (coll.relativeVelocity.magnitude > 3) {
				objectDamage ((int)coll.relativeVelocity.magnitude / 2);
			}
		}
	}

	void objectDamage(int damage){
		if (obj_health > 0) {
			obj_health-=damage;
			if (obj_health <= 0) {
				explode ();
			}
		}
	}

	void explode(){
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		spr.enabled = false;
		if (explosion != null)
			explosion.SetActive (true);
		else
			gameObject.SetActive (false);
	}
}
