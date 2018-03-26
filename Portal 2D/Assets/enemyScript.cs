using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour {
	public int health;

	void enemyDamage(int dmg){
		if (health > 0) {
			health-=dmg;
		} 
		if (health <= 0) {
			gameObject.SetActive (false);
		}
	}
}
