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


    private Rigidbody2D rb;
    private CircleCollider2D col;
    private SpriteRenderer sr;

    private Collider2D targetCol;

    private int invokeCount;
    private int bigInvokeCount;

    private GameObject newBullet;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        targetCol = target.GetComponent<BoxCollider2D>();

        if (target == null)
        {
            Debug.LogError("No player found? HELPPP!");
            return;
        }

        InvokeRepeating("fireMissiles", 0f, 4f);
     //   InvokeRepeating("airstrikes", 0f, 4f);
    }

    void Update()
    {
        
    }


    void fireMissiles()
    {
        newBullet = GameObject.FindGameObjectWithTag("Bullet");


        newBullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetPosition = new Vector3(newBullet.transform.position.x, newBullet.transform.position.y + 1000, 0);

        Ray2D r2d = new Ray2D(newBullet.transform.position, targetPosition - newBullet.transform.position);

        Debug.DrawLine(newBullet.transform.position, targetPosition, Color.red, 1f);
        newBullet.GetComponent<bullet>().Initialize(r2d, bulletSpeed, bulletDelay, bulletColor, 1f, bulletSize, bulletDamage); //direction,speed,delay,color,flip?

        bigInvokeCount = 0;
        InvokeRepeating("chaseTarget", 1f, 0.3f);
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
        GameObject missile1 = GameObject.FindGameObjectWithTag("Bullet");
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


        missile1.GetComponent<bullet>().Initialize(r2d1, bulletSpeed, bulletDelay, bulletColor, 1f, bulletSize, bulletDamage); //direction,speed,delay,color,flip?
      //  missile2.GetComponent<bullet>().Initialize(r2d1, bulletSpeed, bulletDelay, bulletColor, 1f, bulletSize, bulletDamage); //direction,speed,delay,color,flip?
      //  missile3.GetComponent<bullet>().Initialize(r2d1, bulletSpeed, bulletDelay, bulletColor, 1f, bulletSize, bulletDamage); //direction,speed,delay,color,flip?
//
    }
}
