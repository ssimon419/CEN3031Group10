using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour {
	public int health;
	public GameObject explosion;
	public bool boss;
	public RectTransform healthBar;

	private SpriteRenderer spr;

	void Awake(){
		spr = GetComponent<SpriteRenderer> ();
	}

	void Update(){
		healthBar.sizeDelta = new Vector2 (health*4, healthBar.sizeDelta.y);
	}

	void enemyDamage(int dmg){
		if (health > 0) {
			health-=dmg;
		} 
		if (health <= 0) {
			spr.enabled = false;
			gameObject.GetComponentInChildren<SpriteRenderer> ().enabled = false;
			if (explosion != null)
				explosion.SetActive (true);
			else
				Destroy (gameObject);
		}
	}
}
