using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ShootingAI : MonoBehaviour {

    public GameObject target;

    public float minDistance;

    public float laserDamage;

    public int playerHealth = 100; // To be removed once mechanics work together

    public float secondsOfLaser;

    public float secondsBetweenRNG;

    public LayerMask whatToHit;


    private LineRenderer lineRenderer;

    private Seeker seeker;

    private Rigidbody2D rb;

    private float distance;

    private bool isHit = false;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = true;

        if (target == null)
        {
            Debug.LogError("No player found? HELPPP!");
            return;
        }
    }

    // Update is called once per frame
    void Update () {
        if (target == null)
        {
            return;
        }
        

        if (minDistance != 0)
        {
            checkDistance();
        }

        if (playerHealth <= 0)
        {
            Destroy(target);
        }

        shoot();
	}

    void checkDistance()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance <= minDistance)
        {
            rb.Sleep();
            seeker.StopAllCoroutines();
            seeker.CancelCurrentPathRequest();
        }
        else
        {
            rb.WakeUp();
        }
    }

    void shoot()
    {
        isHit = false;

        StartCoroutine(RNGWait());

        if (isHit)
        {
            Vector2 firepointPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
            RaycastHit2D shot = Physics2D.Raycast(firepointPosition, targetPosition - firepointPosition, 100, whatToHit);
            Debug.DrawLine(firepointPosition, (targetPosition - firepointPosition) * 100);

            Debug.Log("The predicted bullet hit: " + shot.collider.name);

            if (shot.collider != null)
            {
                Debug.DrawLine(firepointPosition, shot.point, Color.red);

                Debug.Log("Shot is a hit!");

                lineRenderer.SetPosition(0, firepointPosition);
                lineRenderer.SetPosition(1, shot.point);

                StartCoroutine(laserTime());
            }
        }
    }

    IEnumerator RNGWait()
    {
        int hit_calculator = Random.Range(0, 1000);
        Debug.Log("hit_calculator = " + hit_calculator);


        yield return new WaitForSeconds(secondsBetweenRNG);

        if (hit_calculator < 500)
        {
            playerHealth -= ((int)laserDamage);
            isHit = true;
        }

    }

    IEnumerator laserTime()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(secondsOfLaser);
        lineRenderer.enabled = false;
    }
}
