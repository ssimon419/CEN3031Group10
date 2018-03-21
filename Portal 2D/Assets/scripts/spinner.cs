using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinner : MonoBehaviour {

	//bullet spawner that spins around its own center

	private float z;

	public int damage;
	public float speed;
	public bool flip;
	public float delay;
	public Color currentC;
	public bool randomDelay;
	public float size;

	public Transform spawnPoint;
	public float spinRate=0.1f; //higher = more displacement between angles
	private float nextSpin=0.0f;
	public float spinAmt;

	public Transform center;

	public float fireRate = 0.02f;
	private float nextFire = 0.0f;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		//if(!flip) spawnPoint.RotateAround (center.position, Vector3.forward, spinRate);
		//else spawnPoint.RotateAround (center.position, Vector3.forward, spinRate*-1);
		if (Time.time > nextSpin) {
			center.Rotate (Vector3.forward, spinAmt);
			nextSpin = Time.time + spinRate;
		}
		if (Time.time > nextFire) {
			if (delay == 0) {
				anim.SetBool ("fire", true);
				Invoke ("end_fire", 0.05f);
			}
			if (randomDelay)
				delay = Random.Range (1f, 4f);
			Ray2D r2d = new Ray2D (spawnPoint.position, spawnPoint.position - center.position);
			GameObject boolet = pool_manager.heldPools [0].GetPooledObject ();
			boolet.SetActive (true);
			boolet.GetComponent<Transform>().position = spawnPoint.position;
			boolet.GetComponent<bullet>().Initialize (r2d, speed, delay, currentC,1f,size,damage); //direction,speed,delay,color,flip?

			nextFire = Time.time + fireRate;
		}
	}

	void end_fire(){
		anim.SetBool ("fire", false);
	}
}
