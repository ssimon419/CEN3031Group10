using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class bullet : MonoBehaviour {

	public SpriteRenderer spr;
	public bool disrupt;
	public float randomForce;
	public bool persist = false;

	private int damage;
	private Ray2D r2d;
	private float speed;
	private float flip;
	private Vector3 point;
	public bool sticky = false;
	private Rigidbody2D rb2d;

	private bool portaling=false;
	private GameObject portal;
	private Transform portal_orientation;

	// Use this for initialization lol
	public void Initialize(Ray2D r, float s, float fd,Color c,float f,float scl,int d){ //moveInDirection
		damage = d;
		r2d = r;
		speed = s;
		flip = f;
		rb2d = GetComponent<Rigidbody2D> ();
		spr = GetComponent<SpriteRenderer> ();
		spr.color = c;
		gameObject.transform.localScale = new Vector3(scl,scl,scl);
		if(fd >= 0) Invoke ("fireBullet", fd);
	}

	public void Initialize(Color c){
		spr.color = c;
	}

	public void fireBullet(){
		rb2d.velocity = r2d.direction * speed * flip;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("player_hit")) {
			other.gameObject.SendMessage ("playerDamage");
			gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag("Portal")){
			portaling = true;
		} else if (other.gameObject.CompareTag ("enemy")) {
			other.gameObject.SendMessage ("enemyDamage", damage);
			gameObject.SetActive (false);
		}
		else if (other.gameObject.CompareTag ("ground")&&!portaling) {
			gameObject.SetActive (false);
		}
		else if (other.gameObject.CompareTag ("environment")) {
			other.gameObject.SendMessage ("objectDamage",damage);
			gameObject.SetActive (false);
		}
	}
		

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.CompareTag ("Portal")) {
			portaling = false;
		}
	}
}
