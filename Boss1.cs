using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Boss1 : MonoBehaviour {
    //public float minDistance;

  //  public float laserDamage;

    public GameObject target;

    public int playerHealth = 100; // To be removed once mechanics work together

    //public float hit_rate;
   
    public bool isHardMode;

    public float jumpForce = 20f;


    private Seeker seeker;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private SpriteRenderer sr;

    private Vector3 jump = new Vector3(0f, 5f, 0f);
    private Vector3 slam = new Vector3(0f, -5f, 0f);

    private Collider2D targetCol;
    
    // Use this for initialization
    void Awake () {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        targetCol = target.GetComponent<BoxCollider2D>();

        if (target == null)
        {
            Debug.LogError("No player found? HELPPP!");
            return;
        }

//        seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
  //      StartCoroutine(UpdatePath());

        InvokeRepeating("chooseAttack", 0f, 4f);

        

    }
	
	// Update is called once per frame
	void Update () {

        if (playerHealth <= 0)
        {
            Destroy(target);
            CancelInvoke();
        }

        if (col.IsTouching(targetCol))
        {
            Destroy(target);
        }
    }


    void chooseAttack()
    {
        /*int choosenAttack = Random.Range(1, 3);

        if (choosenAttack == 1)
        {
            attack1();
            Debug.Log("Attack 1 was chosen!");
        }
        else if (choosenAttack == 2)
        {
            attack2();
            Debug.Log("Attack 2 was chosen!");
        }
        else if (choosenAttack == 3)
        {
            attack3();
            Debug.Log("Attack 3 was chosen!");
        }
        else
        {
            Debug.LogError("No error was chosen!");
        }
        */
        attack1();
    }
    // ground slam
    void attack1()
    {
     //   sr.color = Color.white;
        // ground slam
        //       Vector3 dir = (target);
        rb.gravityScale = 0;
        StartCoroutine(changeColor());
        rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
        // for (int i = 0; i < 1; i++) 
        //transform.Translate(Vector3.up * 20 * Time.deltaTime, Space.World);
        // rb.gravityScale = 0;

        

      //  rb.gravityScale = 10;
    //   col.isTrigger = true;
        rb.AddForce(slam, ForceMode2D.Impulse);

        //   sr.color = Color.cyan;
        rb.gravityScale = 1;
        // transform.Translate(Vector3.up * 260 * Time.deltaTime, Space.World);
    }

    IEnumerator changeColor()
    {
        sr.color = Color.white;
        yield return new WaitForSecondsRealtime(1f);
        sr.color = Color.gray;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(other);
        }
    }

    void attack2()
    {
        // shooting
    }

    void attack3()
    {
        // melee
    }


   
}
