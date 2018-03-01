using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class new_portal : MonoBehaviour {

	public new_portal otherPortal;
	public Transform away;

	private bool being_used=false;
	private Collider2D myColl;
	private Vector2 portal_normal;

	// Use this for initialization
	void Awake () {
		myColl = gameObject.GetComponent<Collider2D> ();
		portal_normal = new Vector2 (away.position.x - transform.position.x, away.position.y - transform.position.y).normalized;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!being_used && otherPortal.being_used) {
			return;
		} else if (!being_used && !otherPortal.being_used){
			being_used = true;
			float vel = other.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude;
			other.gameObject.transform.position = otherPortal.transform.position;
			other.gameObject.GetComponent<Rigidbody2D> ().velocity = vel * otherPortal.portal_normal;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (!being_used && otherPortal.being_used) {
			other.enabled = true;
			otherPortal.being_used = false;
		}
	}
}
