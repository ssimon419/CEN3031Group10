using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {


    Rigidbody2D body;

    public float speed = 5.0f;
    public float jumpStrength = 5.0f;


	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        
        

        bool jump = v > 0;

        h *= speed;

        if (jump)
            v = jumpStrength;
        else
            v = body.velocity.y;

    //    Vector2 mouse = Input.mousePosition;


        Vector3 direction = new Vector3(h, v, 0);

        body.velocity = direction;
    }
    
    void Update () {
		
	}
}
