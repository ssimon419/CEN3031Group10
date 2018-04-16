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

    public float bulletSpeed;
    public float bulletDelay;
    public Color bulletColor;
    public float bulletSize;
    public int bulletDamage; 


    private Seeker seeker;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private SpriteRenderer sr;

    private Vector3 jump = new Vector3(0f, 5f, 0f);
    private Vector3 slam = new Vector3(0f, -5f, 0f);

    private Collider2D targetCol;

    private int invokeCount;
    private int bigInvokeCount;

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
        col.radius = 0.5f;
        sr.color = Color.cyan;
        int choosenAttack = Random.Range(1, 4);

        if (choosenAttack == 1)
        {
            attack1();
            Debug.Log("Attack 1 was chosen!");
        }
        else if (choosenAttack == 2)
        {
            bigInvokeCount = 0;
            InvokeRepeating("attack2", 0f, 2f);
            Debug.Log("Attack 2 was chosen!");
        }
        else if (choosenAttack == 3)
        {
            sr.color = Color.green;
          //  StartCoroutine(meleeYeild());
            bigInvokeCount = 0;
            InvokeRepeating("attack3", 2f, 3f);
            Debug.Log("Attack 3 was chosen!");
            col.radius = 0.5f;
        }
        else
        {
            Debug.LogError("No error was chosen!");
        }
        
        
    }

  /*  IEnumerator meleeYeild()
    {
        Debug.Log("Waiting 2 seconds");
        yield return new WaitForSeconds(2);
    }
    */

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
        sr.color = Color.red;
        bigInvokeCount++;
        if (bigInvokeCount >= 3)
            CancelInvoke("attack2");
        invokeCount = 0;
        InvokeRepeating("fireBullet", 0f, .2f);
    }

    void fireBullet()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, 0);

        GameObject newBullet = GameObject.FindGameObjectWithTag("Bullet");


        newBullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Ray2D r2d = new Ray2D(newBullet.transform.position, targetPosition - newBullet.transform.position);

        Debug.DrawLine(newBullet.transform.position, targetPosition, Color.red, 1f);
        newBullet.GetComponent<bullet>().Initialize(r2d, bulletSpeed, bulletDelay, bulletColor, 1f, bulletSize, bulletDamage); //direction,speed,delay,color,flip?

        invokeCount++;
        if (invokeCount >= 2)
            CancelInvoke("fireBullet");
}

    void attack3()
    {
        sr.color = Color.black;

        bigInvokeCount++;
        if (bigInvokeCount >= 1)
            CancelInvoke("attack3");
        col.radius = 1f;
    }

    /*
     * invokerepeating with a wait delay
     * raycast to target on repeat
     * 
     * 
     * */
   
}
