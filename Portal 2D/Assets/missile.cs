using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class missile : MonoBehaviour {

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
	private Transform tip;

	public GameObject explosion;

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
		spr.enabled = true;
		gameObject.transform.localScale = new Vector3(scl,scl,scl);
		if(fd >= 0) Invoke ("fireBullet", fd);
		transform.rotation = Quaternion.LookRotation (Vector3.forward, r2d.direction);
	}

	void Awake(){
		tip = transform.Find ("tip");
		if (tip == null)
			Debug.Log ("tip not found?");
	}

	void Update(){
		if (homing) {
			Quaternion targRot = Quaternion.LookRotation (new Vector3 (0.0f, 0.0f, 1f), target.transform.position - gameObject.transform.position);
			float str = Mathf.Min (homing_rate * Time.deltaTime, 1);
			gameObject.transform.rotation = Quaternion.Lerp (gameObject.transform.rotation, targRot, str);
			updateDirection (new Ray2D (transform.position, tip.position - transform.position));
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
		spr.enabled = true;
		gameObject.transform.localScale = new Vector3(scl,scl,scl);
		if(fd >= 0) Invoke ("fireBullet", fd);
		if (hdc >= 0)
			Invoke ("stopHoming", hdc+hd);
		transform.rotation = Quaternion.LookRotation (Vector3.forward, r2d.direction);
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

	public void updateDirection(Ray2D ray){ //re-fire technique
		r2d = ray;
		fireBullet ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("player_hit") && !friendly) {
			explode ();
		} else if (other.gameObject.CompareTag("Portal")){
			portaling = true;
			friendly = true;
		} else if (other.gameObject.CompareTag ("enemy")&&friendly) {
			other.gameObject.SendMessage ("enemyDamage", damage);
			explode ();
		}
		else if (other.gameObject.CompareTag ("ground")&&!portaling) {
			explode ();
		}
		else if (other.gameObject.CompareTag ("environment")) {
			other.gameObject.SendMessage ("objectDamage",damage);
			explode ();
		}
	}


	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.CompareTag ("Portal")) {
			portaling = false;
			friendly = true;
			if (homing) {
				target = GameObject.FindWithTag ("enemy").transform;
				homing = true;
				Invoke ("stopHoming", Random.Range (0.5f, 2f));
			}
		}
	}

	void explode(){
		rb2d.velocity = Vector2.zero;
		homing = false;
		spr.enabled = false;
		if (explosion != null)
			explosion.SetActive (true);
		else
			gameObject.SetActive (false);
	}
}
