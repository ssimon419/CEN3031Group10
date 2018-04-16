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
	private Transform target;

	private bool friendly=false;
	private bool portaling=false;
	private bool homing=false;
	private float homing_rate=0.5f;
	private float nextCalc=0.0f;
	private GameObject portal;
	private Transform portal_orientation;

	// Use this for initialization lol
	public void Initialize(Ray2D r, float s, float fd,Color c,float f,float scl,int d){ //moveInDirection
		friendly=false;
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

	void Update(){
		if (Time.time > nextCalc && homing) {
			r2d = new Ray2D(transform.position, target.position - transform.position);
			updateDirection (r2d);
			nextCalc = Time.time + homing_rate;
		}
	}

	public void Initialize(Color c){
		spr.color = c;
	}

	public void Initialize(Transform targ, Ray2D r, float s, float hr, float fd, float hd, float hdc, Color c,float f,float scl,int d){ //homing bullet initializer
		target=targ;
		if (hd >= 0)
			Invoke ("startHoming", hd);
		else homing = true;
		homing_rate = hr;
		friendly=false;
		damage = d;
		r2d = r; //direction
		speed = s;
		flip = f; 
		rb2d = GetComponent<Rigidbody2D> ();
		spr = GetComponent<SpriteRenderer> ();
		spr.color = c;
		gameObject.transform.localScale = new Vector3(scl,scl,scl);
		if(fd >= 0) Invoke ("fireBullet", fd);
		if (hdc >= 0)
			Invoke ("stopHoming", hdc+hd);
	}

	private void startHoming(){
		homing = true;
	}

	private void stopHoming(){
		homing = false;
	}

	public void fireBullet(){
		rb2d.velocity = r2d.direction * speed * flip;
	}

	public void updateDirection(Ray2D ray){
		r2d = ray;
		rb2d.velocity = r2d.direction * speed * flip;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("player_hit") && !friendly) {
			other.gameObject.SendMessage ("playerDamage",damage);
			gameObject.SetActive (false);
		} else if (other.gameObject.CompareTag("Portal")){
			portaling = true;
			friendly = true;
		} else if (other.gameObject.CompareTag ("enemy")&&friendly) {
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
			friendly = true;
			if (homing) {
				target = GameObject.FindWithTag ("enemy").transform;
				startHoming ();
				Invoke ("stopHoming", Random.Range (0.5f, 2f));
			}
		}
	}
}
