using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour {

	public int health = 100;
	public GameObject player;
	public RectTransform healthBar;

	private Animator p_Anim;
	private bool invincible=false;

	void Update(){
		healthBar.sizeDelta = new Vector2 (health, healthBar.sizeDelta.y);
	}

	// Use this for initialization
	public void playerDamage(int dmg){
		if (health > 0 && !invincible) {
			health -= 10;
			invincible = true;
			Invoke ("cancelInvincible", 3f);
		} else if (health <= 0 && !invincible) {
			playerLose ();
		}
	}

	void cancelInvincible(){
		invincible = false;
	}

	void playerLose(){
		player.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("pain")) {
			playerDamage(10);
		}
	}

}
