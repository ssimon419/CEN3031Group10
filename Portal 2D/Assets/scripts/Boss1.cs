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
   
	public Color nice_green;
    public bool isHardMode;

    public float jumpForce = 20f;

    public float bulletSpeed;
    public float bulletDelay;
    public Color bulletColor;
    public float bulletSize;
    public int bulletDamage; 

	public arenaHandler arena;

	private bool attacking=false;
	private bool face_right = true;
    private Seeker seeker;
    private Rigidbody2D rb;
	private Animator m_anim;
	private GameObject field;

    private Collider2D targetCol;

	private Transform fire_p;
    private int invokeCount;
    private int bigInvokeCount;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
	private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
	private bool is_grounded;

    // Use this for initialization
    void Awake () {
		m_GroundCheck = transform.Find ("GroundCheck");
		fire_p = transform.Find ("fire_pos");
		m_anim = GetComponent<Animator> ();
		field = transform.Find ("defense_field").gameObject;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
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
		m_anim.SetFloat ("hSpeed", rb.velocity.x);
		m_anim.SetFloat ("vSpeed", rb.velocity.y);
		if(face_right && ((transform.position.x - target.transform.position.x)>0)){
			flip();
		} else if(!face_right && ((transform.position.x - target.transform.position.x)<0)){
			flip();
		}
    }

	void FixedUpdate(){
		is_grounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders [i].gameObject != gameObject) {
				is_grounded = true;
			}
		}
		m_anim.SetBool ("jump", !is_grounded);
	}


    void chooseAttack()
    {
		int choosenAttack = Random.Range(1, 4);
		if (!attacking) {
			if (choosenAttack == 1) {
				field.GetComponent<SpriteRenderer> ().color = nice_green;
				attacking = true;
				attack1 ();
				Debug.Log ("Attack 1 was chosen!");
			} else if (choosenAttack == 2) {
				field.GetComponent<SpriteRenderer> ().color = Color.cyan;
				Debug.Log ("Attack 2 was chosen!");
				attacking = true;
				m_anim.SetBool ("aim", true);
			
			} else if (choosenAttack == 3) {
				//  StartCoroutine(meleeYeild());
				/*bigInvokeCount = 0;
				InvokeRepeating ("attack3", 2f, 3f);*/
				m_anim.SetBool ("defense", true);
				field.GetComponent<SpriteRenderer> ().color = Color.green;
				attacking = true;
				Debug.Log ("Attack 3 was chosen!");
			} else {
				Debug.LogError ("No error was chosen!");
			}
		}    
    }

  /*  IEnumerator meleeYeild()
    {
        Debug.Log("Waiting 2 seconds");
        yield return new WaitForSeconds(2);
    }
    */

	void swap_arena(){
		field.GetComponent<Animator> ().SetBool ("swap", true);
		attacking = false;
		arena.swap();
	}

    // ground slam
    void attack1()
    {
     //   sr.color = Color.white;
        // ground slam
        //       Vector3 dir = (target);
		is_grounded=false;
		m_anim.SetBool("jump",true);
		Vector2 jump = new Vector2(target.transform.position.x - transform.position.x, 10f).normalized;
        rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
        // for (int i = 0; i < 1; i++) 
        //transform.Translate(Vector3.up * 20 * Time.deltaTime, Space.World);
        // rb.gravityScale = 0;

        

      //  rb.gravityScale = 10;
    //   col.isTrigger = true;
        //rb.AddForce(slam, ForceMode2D.Impulse);

        //   sr.color = Color.cyan;
        //rb.gravityScale = 1;
        // transform.Translate(Vector3.up * 260 * Time.deltaTime, Space.World);
    }

    IEnumerator changeColor()
    {
        yield return new WaitForSecondsRealtime(1f);
    }

	void flip(){
		face_right = !face_right;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    void attack2()
	{
		m_anim.SetBool ("fire", true);
		invokeCount = 0;
    }

	void cancel_attack2(){
		m_anim.SetBool ("aim", false);
		Debug.Log ("cancelled attack 2");
		attacking = false;
	}


    void fireBullet()
    {
		GameObject newBullet = pool_manager.heldPools [0].GetPooledObject ();
		newBullet.transform.position = fire_p.position;
		Vector3 fp_mod = new Vector3(fire_p.position.x,fire_p.position.y+(Random.Range(-1f,1f)*0.1f));
		Ray2D r2d = new Ray2D (transform.position, fp_mod-transform.position);
		newBullet.SetActive (true);
		if(invokeCount!=10) newBullet.GetComponent<bullet>().Initialize(r2d, bulletSpeed+invokeCount/2, bulletDelay, bulletColor, 1f, bulletSize, bulletDamage); //direction,speed,delay,color,flip?
		else {
			rb.AddForce (new Vector2 (transform.position.x - target.transform.position.x, 0.02f).normalized* 50f,ForceMode2D.Impulse);
			newBullet.GetComponent<bullet>().Initialize(r2d, bulletSpeed*3f, bulletDelay, Color.cyan, 1f, bulletSize*2f, bulletDamage); //direction,speed,delay,color,flip?
		}

        invokeCount++;
		if (invokeCount > 10) {
			m_anim.SetBool ("fire", false);
		}
	}

    void attack3()
    {
		
		field.GetComponent<Animator> ().SetBool ("defense", true);
		m_anim.SetBool ("defense", false);
		attacking = false;
        /*bigInvokeCount++;
        if (bigInvokeCount >= 1)
            CancelInvoke("attack3");*/
    }
    /*
     * invokerepeating with a wait delay
     * raycast to target on repeat
     * 
     * 
     * */
   
}
