using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimingSpawner : MonoBehaviour {

	public Transform source;
	public Transform center;
	public Transform target;
	public int damage;
	public bool burst;
	public float burstAmount;
	public float burstDelay;
	public Color c;
	public bool enlargeFirst;
	public float initDelay;
	public float bulletSpd;
	public float fireRate;
	public float size;
	public float rotSpeed;
	public bool gatling;
	public float spread=0f;
	public bool ramp_up;
	public float final_rate;

	private float nextFire=0f;
	private bool activating=false;
	private bool aiming=false;
	private float curExtend=0f;
	public Transform turret_base;
	private Animator anim;
	private bool first=false;
	
	// Update is called once per frame'

	public void Initialize_Gatling(Transform tr, int dmg, Color col, float d, float s, float r, float sz, float rs, float spr){ //gatling rapid
		target = tr;
		damage = dmg;
		c = col;
		initDelay = d;
		bulletSpd = s;
		fireRate = r;
		size = sz;
		rotSpeed = rs;
		spread = spr;
	}

	public void Initialize_Gatling(Transform tr, int dmg, Color col, float d, float s, float r, float sz, float rs, float spr, float bAmt, float bDel, bool enF){ //gatling burst
		burst = true;
		burstAmount = bAmt;
		burstDelay = bDel;
		enlargeFirst = enF;

		target = tr;
		damage = dmg;
		c = col;
		initDelay = d;
		bulletSpd = s;
		fireRate = r;
		size = sz;
		rotSpeed = rs;
		spread = spr;
	}

	void Awake(){
		anim = GetComponent<Animator> ();

	}

	void OnEnable () {
		aiming = false;
		activating = true;
		curExtend = 0f;
	}

	void OnDisable(){
		aiming = false;
		for (float i = 0; i < 3.8f; i += 0.05f) {
			turret_base.Translate (0f, -0.05f, 0f);
			curExtend += 0.05f;
		}
		CancelInvoke ();
	}

	void Update(){
		if (activating) {
			if (curExtend < 3.8f) {
				turret_base.Translate (0f, 0.05f, 0f);
				curExtend += 0.05f;
			} else {
				activating = false;	
				aiming = true;
			}
		} 
		if(aiming) {
			if (Time.time > nextFire) {
				ShootBullet ();
				nextFire = Time.time + fireRate;
			}
			Quaternion targRot = Quaternion.LookRotation (new Vector3 (0.0f, 0.0f, 1f), target.position - gameObject.transform.position);
			float str = Mathf.Min (rotSpeed * Time.deltaTime, 1);
			gameObject.transform.rotation = Quaternion.Lerp (gameObject.transform.rotation, targRot, str);
		}
	}

	void ShootBullet () {
		if (!burst) {
			SpawnBullet ();
		} else {
			float timer = burstDelay * burstAmount;
			if (gatling) {
				anim.SetBool ("fire", true);
			}
			if(enlargeFirst) first = true;
			for (int i = 0; i < burstAmount; i++) {
				Invoke ("SpawnBullet", timer);
				timer -= burstDelay;
			}
			anim.SetBool ("fire", false);
		}
	}

	void SpawnBullet(){
		if (!gatling) {
			anim.SetBool ("fire", true);
			Invoke ("end_fire", 0.05f);
		}
		GameObject boolet = pool_manager.heldPools [0].GetPooledObject ();
		Vector3 new_pos = source.position;
		if (spread > 0f) {
			new_pos = new Vector3 (source.position.x + Random.Range (spread * -1f, spread),source.position.y+ Random.Range (spread * -1f, spread));
		}
		Ray2D r2d = new Ray2D (center.position, new_pos - center.position);
		if (burst && first) {
			boolet.SetActive (true);
			boolet.GetComponent<Transform>().position = source.position;
			boolet.GetComponent<bullet>().Initialize (r2d, bulletSpd, 0f, c, 1f, size*2,damage);
			first = false;
		} else {
			boolet.SetActive (true);
			boolet.GetComponent<Transform>().position = source.position;
			boolet.GetComponent<bullet>().Initialize (r2d, bulletSpd, 0f, c, 1f, size,damage);
		}
		if (ramp_up && fireRate > final_rate) {
			Debug.Log (fireRate);
			fireRate -= 0.01f;
		}
	}

	void end_fire(){
		anim.SetBool("fire", false);
	}
}
