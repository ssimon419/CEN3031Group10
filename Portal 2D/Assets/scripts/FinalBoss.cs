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
		if (!attacking) {
			int chosenAttack = Random.Range (1, 5);
			//int chosenAttack=999;
			if (chosenAttack == 1) {
				Debug.Log ("chose missiles");
				missiles = true;
				transform.position = positions [0].position;
				InvokeRepeating("fireMissiles", 0f, 0.8f);
			} if (chosenAttack == 2) {
				Debug.Log ("turret setup");
				turret_atk = true;
				transform.position = positions [4].position;
			} else if (chosenAttack == 3) {
				Debug.Log ("chose airstrikes");
				airstrikes = true;
			} else {
				Debug.Log ("launching train");
				arena.GetComponent<arenaHandler> ().swap (3,5);
				train.GetComponent<destructible_object> ().obj_health = 50;
				if (Random.Range (1, 3) == 1) {
					train.GetComponent<destructible_object> ().obj_health = 9999;
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
			} else if (train_launch>=1) {
				train_time ();
			} else if (airstrikes) {
				airstrikes = false;
			} else if (turret_atk) {
				if (transform.position.x < positions [1].position.x) {
					transform.Translate (0.15f, 0f, 0f);
				}
				turret_atk = false;
			} else {
				CancelInvoke ();
				attacking = false;
			}
		}
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
			++train_launch;
		} else if (train_launch == 2) {
			if (count > 5f) {
				++train_launch;
			} else if (Time.time > nextTime) {
				if (!train_alt)
					train.transform.localScale = new Vector3 (count, count, count);
				else {
					for (int i = 0; i < 3; ++i) {
						float curVal = Mathf.Min (count, 2.4f);
						train.transform.localScale = new Vector3 (curVal, curVal, curVal);
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
				train.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (1500f, 0f), ForceMode2D.Impulse);
			count = 0f;
			nextTime = Time.time + 5f;
			train_launch = 4;
		} else if (train_launch == 4) {
			if (count > 5f) {
				train.transform.localScale = new Vector3(0f, 0f, 0f);
				train.transform.position= transform.position;
				train.SetActive (false);
				train_alt = false;
				train_launch = 0;
				arena.GetComponent<arenaHandler> ().swap (3,5);
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


    void fireMissiles()
    {
		GameObject boolet = pool_manager.heldPools [2].GetPooledObject ();


        boolet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetPosition = new Vector3(boolet.transform.position.x, boolet.transform.position.y + 1000, 0);

        Ray2D r2d = new Ray2D(boolet.transform.position, targetPosition - boolet.transform.position);
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


        missile1.transform.position = new Vector3(target.transform.position.x, target.transform.position.y+1, transform.position.z);
     //   missile2.transform.position = new Vector3(target.transform.position.x, target.transform.position.y+1, transform.position.z);
       // missile3.transform.position = new Vector3(target.transform.position.x, target.transform.position.y+1, transform.position.z);

        Vector3 targetPosition1 = new Vector3(target.transform.position.x - 10, target.transform.position.y, 0);
      //  Vector3 targetPosition2 = new Vector3(target.transform.position.x, target.transform.position.y, 0);
      //  Vector3 targetPosition3 = new Vector3(target.transform.position.x + 10, target.transform.position.y, 0);


        Ray2D r2d1 = new Ray2D(missile1.transform.position, targetPosition1 - missile1.transform.position);
     //   Ray2D r2d2 = new Ray2D(missile2.transform.position, targetPosition1 - missile2.transform.position);
     //   Ray2D r2d3 = new Ray2D(missile3.transform.position, targetPosition1 - missile3.transform.position);


        Debug.DrawLine(missile1.transform.position, targetPosition1, Color.red, 1f);
     //   Debug.DrawLine(missile2.transform.position, targetPosition2, Color.red, 1f);
      //  Debug.DrawLine(missile3.transform.position, targetPosition3, Color.red, 1f);

		missile1.SetActive (true);
        missile1.GetComponent<bullet>().Initialize(r2d1, bulletSpeed, bulletDelay, bulletColor, 1f, bulletSize, bulletDamage); //direction,speed,delay,color,flip?
      //  missile2.GetComponent<bullet>().Initialize(r2d1, bulletSpeed, bulletDelay, bulletColor, 1f, bulletSize, bulletDamage); //direction,speed,delay,color,flip?
      //  missile3.GetComponent<bullet>().Initialize(r2d1, bulletSpeed, bulletDelay, bulletColor, 1f, bulletSize, bulletDamage); //direction,speed,delay,color,flip?
//
    }
}
