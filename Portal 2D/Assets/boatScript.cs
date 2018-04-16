using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boatScript : MonoBehaviour {

    public float speed;

    private bool moving = false;

    private Transform trans;

    public CameraRail camera;

    private Rigidbody2D body;

    public Transform boatEnd;

	void Start () {
        trans = GetComponent<Transform>();
       // body = GetComponent<Rigidbody2D>();

        // beginMoving();
    }


    void FixedUpdate () {
        if (moving)
        {
            trans.position = new Vector3(trans.position.x + speed, trans.position.y, trans.position.z);
            if(trans.position.x > boatEnd.position.x)
            {
                moving = false;
                camera.setRail(null);
            }

        }
       
	}

    
   
    public void beginMoving()
    {
        moving = true;
        camera.setRail(trans);
       // body.velocity = (new Vector3(speed, 0 , 0));
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(transform);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
       collision.collider.transform.SetParent(null);
    }
}
