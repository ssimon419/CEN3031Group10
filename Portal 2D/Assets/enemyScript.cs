using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour {
	public int health;
	private int maxHealth;
	public GameObject explosion;
	public bool boss;
	public RectTransform healthBar;

	public GameObject exit_object;

	private SpriteRenderer spr;

	void Awake(){
		maxHealth = health;
		spr = GetComponent<SpriteRenderer> ();
	}

	void Update(){
		if(boss) healthBar.sizeDelta = new Vector2 (health, healthBar.sizeDelta.y);
	}

	public void enemyDamage(int dmg){
		if (health > 0) {
			health-=dmg;
		} 
		if (health <= 0) {
			if (boss)
				exit_object.SetActive(true);
			spr.enabled = false;
			gameObject.GetComponentInChildren<SpriteRenderer> ().enabled = false;
			if (explosion != null)
				explosion.SetActive (true);
			else
				Destroy (gameObject);
		}
	}
}
