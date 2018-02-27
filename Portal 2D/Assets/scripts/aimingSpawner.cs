using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimingSpawner : MonoBehaviour {

	public Transform source;
	public Transform center;
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

	private GameObject target;
	private bool first=false;
	
	// Update is called once per frame

	void Awake(){
		target = GameObject.FindGameObjectWithTag ("hitbox");
		InvokeRepeating ("ShootBullet", initDelay, fireRate);
	}

	void Update(){
		Quaternion targRot = Quaternion.LookRotation (new Vector3(0.0f,0.0f,1f),target.transform.position - center.position);
		float str = Mathf.Min (rotSpeed * Time.deltaTime, 1);
		center.rotation=Quaternion.Lerp(center.rotation,targRot,str);
	}

	void ShootBullet () {
		if (!burst) {
			SpawnBullet ();
		} else {
			float timer = burstDelay * burstAmount;
			if(enlargeFirst) first = true;
			for (int i = 0; i < burstAmount; i++) {
				Invoke ("SpawnBullet", timer);
				timer -= burstDelay;
			}
		}
	}

	void SpawnBullet(){
		GameObject boolet = pool_manager.heldPools [0].GetPooledObject ();
		Ray2D r2d = new Ray2D (center.position, source.position - center.position);
		if (burst && first) {
			boolet.SetActive (true);
			boolet.GetComponent<Transform>().position = source.position;
			boolet.GetComponent<bullet>().Initialize (r2d, bulletSpd, 0f, Color.cyan, 1f, size*2);
			first = false;
		} else {
			boolet.SetActive (true);
			boolet.GetComponent<Transform>().position = source.position;
			boolet.GetComponent<bullet>().Initialize (r2d, bulletSpd, 0f, c, 1f, size);
		}
	}

	void OnDisable(){
		CancelInvoke ();
	}
}
