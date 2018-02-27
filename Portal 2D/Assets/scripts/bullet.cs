using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class bullet : MonoBehaviour {

	public SpriteRenderer spr;
	public bool disrupt;
	public float randomForce;
	public bool persist = false;

	private Ray2D r2d;
	private float speed;
	private float flip;
	private Vector3 point;
	public bool sticky = false;
	private Rigidbody2D rb2d;

	// Use this for initialization lol
	public void Initialize(Ray2D r, float s, float fd,Color c,float f,float scl){ //moveInDirection
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
		if (other.gameObject.CompareTag ("hitbox")) {
			other.gameObject.SendMessage ("playerDamage");
		}
		/*if (other.gameObject.CompareTag ("PAIN") && disrupt) {
			other.GetComponent<Rigidbody2D> ().velocity = new Vector2 (Random.Range (-1f, 1f), Random.Range (-1f, 1f)) * randomForce;
		} else if (other.gameObject.CompareTag ("bulletAffector") && sticky) {
			other.GetComponent<Rigidbody2D> ().velocity = rb2d.velocity;
		} else if (other.gameObject.CompareTag ("border") && persist) {
			float ranTheta = Mathf.Deg2Rad * Random.Range (-30f, 30f);
			float x = rb2d.velocity.x;
			float y = rb2d.velocity.y;
			rb2d.velocity = new Vector2 (x * Mathf.Cos (ranTheta) - y * Mathf.Sin (ranTheta), x * Mathf.Sin (ranTheta) + y * Mathf.Cos (ranTheta)) * -1f;
		} else if (other.gameObject.CompareTag ("sticky") && sticky) {
			rb2d.velocity = Vector2.zero;
		} else */ //can be added before previous to do some special things i defined a while back
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.CompareTag ("MainCamera") && !persist) {
			gameObject.SetActive (false);
		}
	}

	void OnBecameInvisible() {
		gameObject.SetActive (false);
	}
}
