using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss1_defense : MonoBehaviour {

	private Animator anim;

	private CircleCollider2D c_col;
	public activation_radius turrets;

	private bool absorb;

	// Use this for initialization
	void Awake() {
		anim = GetComponent<Animator> ();

	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("trail")) {
			for (int i = 0; i < 15; ++i) {
				GameObject newBullet = pool_manager.heldPools [0].GetPooledObject ();
				newBullet.transform.position = other.transform.position;
				Ray2D r2d = new Ray2D (Vector2.zero, new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f)));
				newBullet.SetActive (true);
				newBullet.GetComponent<bullet> ().Initialize (r2d, 3f, 0f, other.GetComponent<trail>().c, 1f, 2f, 1);
			}
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
		gameObject.tag = "enemy";
	}

	void cancel_swap(){
		turrets.activateObjects ();
		for (int i = 0; i < 50; ++i) {
			GameObject newBullet = pool_manager.heldPools [0].GetPooledObject ();
			newBullet.transform.position = transform.position+(0.2f*Vector3.up);
			Ray2D r2d = new Ray2D (new Vector2(transform.position.x,transform.position.y+0.2f), new Vector2(Random.Range(-1f,1f),Random.Range(0.01f,1f)));
			newBullet.SetActive (true);
			newBullet.GetComponent<bullet> ().Initialize (r2d, Random.Range(5f,10f), 0f, Color.black, 1f, 3f, 1);
		}
		anim.SetBool ("swap", false);
	}
}
