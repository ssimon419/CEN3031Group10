using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour {

	public int health = 3;
	public GameObject player;

	private Animator p_Anim;
	private bool invincible=false;

	// Use this for initialization
	void playerDamage(int dmg){
		if (health > 0 && !invincible) {
			health-=dmg;
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
			playerDamage(3);
		}
	}

}
