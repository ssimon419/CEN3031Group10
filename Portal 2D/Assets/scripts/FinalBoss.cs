using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour {

    public GameObject target;

    public float bulletSpeed;
    public float bulletDelay;
    public Color bulletColor;
    public float bulletSize;
    public int bulletDamage;
	public GameObject arena;
	public GameObject train;

	[SerializeField] private LayerMask m_WhatIsGround;

    private Rigidbody2D rb;
    private CircleCollider2D col;
    private SpriteRenderer sr;

    private Collider2D targetCol;

    private int invokeCount;
    private int bigInvokeCount;

	private bool attacking=false;

    private GameObject newBullet;
	public Transform[] positions;
	public GameObject[] turrets;
	private bool missiles=false;
	private bool airstrikes=false;
	private bool turret_atk=false;
	private bool train_alt=false;
	private bool exploding=false;
	private int train_launch=0;
	private float count = 0f;
	private float nextTime = 0.0f;

    void Awake()
    {
		train.transform.localScale.Set (0f, 0f, 0f);
		train.SetActive (false);
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        targetCol = target.GetComponent<BoxCollider2D>();
		transform.position = positions [0].position;
        if (target == null)
        {
            Debug.LogError("No player found? HELPPP!");
            return;
        }
      //  InvokeRepeating("airstrike", 0f, 4f);
    }

    void Update() //0 - bleft 1 - bright 2 - tleft 3 -tright
    {
		if (exploding) {
			if (Time.time > nextTime) {
				count -= 0.01f;
				transform.position = new Vector3 (Random.Range (-40f, 35f), Random.Range (-4f, 9f));
				GameObject exp = pool_manager.heldPools [3].GetPooledObject ();
				exp.SetActive (true);
				exp.transform.position = transform.position;
				exp.transform.localScale = new Vector3 (70f, 70f);
				nextTime = Time.time + count;
			}
		}
		else if (arena.GetComponent<enemyScript> ().health <= 0) {
			for (int i = 0; i < turrets.Length; ++i) {
				turrets [i].SetActive (false);
			}
			CancelInvoke ();
			count = 0.8f;
			exploding = true;
		} else if (!attacking&&!exploding) {
			int chosenAttack = Random.Range (1, 5);
			if (chosenAttack == 1) {
				Debug.Log ("chose missiles");
				missiles = true;
				transform.position = positions [0].position;
				InvokeRepeating("fireMissiles", 0f, 0.8f);
			} else if (chosenAttack == 2) {
				turret_atk = true;
				Debug.Log ("turret setup");
				turret_setup ();
			} else if (chosenAttack == 3) {
				Debug.Log ("chose airstrikes");
				airstrikes = true;
				transform.position = positions [3].position;
				InvokeRepeating ("airstrike", 0f, 0.8f);
				InvokeRepeating ("fireBullet", 0f, 0.1f);
			} else {
				Debug.Log ("launching train");
				arena.GetComponent<arenaHandler> ().swap (3,6);
				train.GetComponent<destructible_object> ().obj_health = 9999;
				if (Random.Range (1, 3) == 1) {
					train_alt = true;
				}
				train.GetComponent<SpriteRenderer> ().enabled = true;
				train_launch = 1;
			}
			attacking = true;
		} else if (attacking) {
			if (missiles) {
				if (transform.position.x < positions [1].position.x) {
					transform.Translate (0.15f, 0f, 0f);
				} else {			
					CancelInvoke ("fireMissiles");
					missiles = false;
				}
			} else if (train_launch >= 1) {
				train_time ();
			} else if (airstrikes) {
				if (transform.position.x > positions [2].position.x) {
					transform.Translate (-0.15f, 0f, 0f);
				} else {	
					transform.position = positions [9].position;
					CancelInvoke ("airstrike");
					final_strike ();
					airstrikes = false;
				}
			} else if (turret_atk) {
				turret_atk = false;
				for (int i = 0; i < turrets.Length; ++i) {
					if (turrets [i].activeInHierarchy)
						turret_atk = true;
				}
			} else {
				CancelInvoke ();
				attacking = false;
			}
		}
    }

	void turret_setup(){
		transform.position = positions [4].position;
		for (int i=0; i<4; ++i) {
			transform.Translate (6f, 0f, 0f);
			turrets [i].SetActive (true);
			turrets [i].transform.position = transform.position;
			if(Random.Range(1,3) ==1) 
				turrets [i].GetComponentInChildren<aimingSpawner> ().Initialize_Gatling (target.transform, 10, Color.yellow, 0f,Random.Range(8f,15f), Random.Range(2f,5f),3f, Random.Range(1f,5f),Random.Range(0f,1f));
			else turrets [i].GetComponentInChildren<aimingSpawner> ().Initialize_Gatling (target.transform, 10, Color.yellow, 0f,Random.Range(8f,15f), Random.Range(2.5f,5f),3f, Random.Range(1f,5f), Random.Range(0f,0.1f),Random.Range(3,10),0.02f,false);
		}
		transform.position = positions [10].position;
		turrets [4].SetActive (true);
		turrets [4].transform.position = transform.position;
		turrets [4].GetComponentInChildren<aimingSpawner> ().Initialize_Gatling (target.transform, 10, Color.yellow, 0f,Random.Range(6f,9f),Random.Range(3f,6f),5f, 0.6f,Random.Range(0f,2f),25,0.04f,true);
	}

	void train_time(){ //all handling for both train attacks :>
		if (train_launch == 1) {
			count = 0f;
			if (!train_alt)
				transform.position = positions [6].position; //throw train
			else
				transform.position = positions [8].position; //fire train
			train.SetActive (true);
			train.transform.position = transform.position;
			train.tag = "environment";
			++train_launch;
		} else if (train_launch == 2) {
			if (count > 5f) {
				train.tag = "ground";
				++train_launch;
			} else if (Time.time > nextTime) {
				if (!train_alt)
					train.transform.localScale = new Vector3 (count, count, count);
				else {
					float curVal = Mathf.Min (count, 2.4f);
					train.transform.localScale = new Vector3 (curVal, curVal, curVal);
					for (int i = 0; i < 3; ++i) {
						arena.GetComponent<arenaHandler> ().platforms [i].transform.Translate (0f, 0.04f, 0f);
					}
				}
				count += 0.05f;
				nextTime = Time.time + 0.05f;
			}
		} else if (train_launch == 3) {
			if (!train_alt)
				train.GetComponent<Rigidbody2D> ().AddForceAtPosition (new Vector2 (300f, 900f), positions [7].position, ForceMode2D.Impulse);
			else
				train.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (1800f, 0f), ForceMode2D.Impulse);
			count = 0f;
			nextTime = Time.time + 5f;
			train_launch = 4;
		} else if (train_launch == 4) {
			if (count > 5f) {
				for (int i = 0; i < train.transform.childCount; ++i) {
					train.transform.GetChild (i).gameObject.SetActive (false);
				}
				train.transform.localScale = new Vector3(0f, 0f, 0f);
				train.transform.position= transform.position;
				train.SetActive (false);
				train_alt = false;
				train_launch = 0;
				arena.GetComponent<arenaHandler> ().swap (3,6);
			} else if (Time.time > nextTime) {
				if (train_alt) {
					for (int i = 0; i < 3; ++i) {
						arena.GetComponent<arenaHandler> ().platforms [i].transform.Translate (0f, -0.04f, 0f);
					}
				}
				count += 0.05f;
				nextTime = Time.time + 0.05f;
			}
		}
	}

	void fireBullet(){
		
			GameObject boolet = pool_manager.heldPools [0].GetPooledObject ();
			boolet.transform.position = transform.position;
			Vector3 mod_pos = new Vector2 (target.transform.position.x + Random.Range (-1f, 1f), target.transform.position.y + Random.Range (-1f, 1f));
			Ray2D r2d = new Ray2D (transform.position, mod_pos - transform.position);
			boolet.SetActive (true);
			boolet.GetComponent<bullet> ().Initialize (r2d, Random.Range(13f,19f), 0f, Color.yellow, 1f,4f,10);
	}

    void fireMissiles()
    {
		GameObject boolet = pool_manager.heldPools [2].GetPooledObject ();


        boolet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		Ray2D r2d = new Ray2D(boolet.transform.position, Vector2.up);
		boolet.SetActive (true);
		boolet.GetComponent<missile>().Initialize(target.transform, r2d, bulletSpeed, 5f, bulletDelay, 2f, 5f, bulletColor, 1f, bulletSize, bulletDamage); 
		//target,init direction, speed, homing rate, fire delay, homing delay, homing decay option, color, flip?, size, damage
    }


    void chaseTarget()
    {
        bigInvokeCount++;
        if (bigInvokeCount >= 5)
            CancelInvoke("chaseTarget");

        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, 0);
        Ray2D r2d = new Ray2D(newBullet.transform.position, targetPosition - newBullet.transform.position);
        Debug.DrawLine(newBullet.transform.position, targetPosition, Color.cyan, 1f);

        newBullet.GetComponent<bullet>().updateDirection(r2d);
    }


    void airstrike ()
    {
		GameObject missile1 = pool_manager.heldPools [2].GetPooledObject ();
      //  GameObject missile2 = GameObject.FindGameObjectWithTag("Bullet");
     //   GameObject missile3 = GameObject.FindGameObjectWithTag("Bullet");

		Ray2D r2d1 = new Ray2D(transform.position, new Vector2(-1f,-4f));

		RaycastHit2D rHit = Physics2D.Raycast (r2d1.origin, r2d1.direction, 300,m_WhatIsGround);
		GameObject indr = pool_manager.heldPools [1].GetPooledObject ();
		indr.SetActive (true);
		indr.transform.localScale = new Vector3 (20f, 20f, 20f);
		indr.transform.position = rHit.point;

		missile1.SetActive (true);
		missile1.transform.position =(transform.position);
		missile1.GetComponent<missile>().Initialize(r2d1, 25f, bulletDelay, bulletColor, 1f, bulletSize*2, bulletDamage); //direction,speed,delay,color,flip?
//
    }

	void final_strike(){
		GameObject missile1 = pool_manager.heldPools [2].GetPooledObject ();
		//  GameObject missile2 = GameObject.FindGameObjectWithTag("Bullet");
		//   GameObject missile3 = GameObject.FindGameObjectWithTag("Bullet");

		Ray2D r2d1 = new Ray2D(transform.position, new Vector2(0f,-1f));

		RaycastHit2D rHit = Physics2D.Raycast (r2d1.origin, r2d1.direction, 300);
		GameObject indr = pool_manager.heldPools [1].GetPooledObject ();
		indr.SetActive (true);
		indr.transform.localScale = new Vector3 (40f, 40f, 40f);
		indr.transform.position = rHit.point;

		missile1.SetActive (true);
		missile1.transform.position =(transform.position);
		missile1.GetComponent<missile>().Initialize(r2d1, 15f, bulletDelay, bulletColor, 1f, bulletSize*3, bulletDamage); //direction,speed,delay,color,flip?
	}
}
