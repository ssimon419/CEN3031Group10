using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {


    Rigidbody2D body;

    public float speed = 5.0f;
    public float jumpStrength = 5.0f;

    public float terminalVelocity;


	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
       

	}

    void FixedUpdate()
    {


        BoxCollider2D coll = body.GetComponent<BoxCollider2D>();



       // coll.size = new Vector2(coll.size.x+0.1f, coll.size.y);
        //coll.offset = new Vector2(coll.offset.x + 0.05f, coll.offset.y);



        //  coll.bounds.SetMinMax(new Vector3(coll.bounds.min.x - 1f, coll.bounds.min.y, 0), new Vector3(coll.bounds.max.x + 1f, coll.bounds.max.y, 0));


        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        
        

        bool jump = v > 0;

        h *= speed;

        if (jump)
            v = jumpStrength;
        else
            v = body.velocity.y;

        if(v < terminalVelocity * -1)
            v = terminalVelocity * -1;

    //    Vector2 mouse = Input.mousePosition;


        Vector3 direction = new Vector3(h, v, 0);

        body.velocity = direction;
    }
    
    void Update () {
		
	}
}
