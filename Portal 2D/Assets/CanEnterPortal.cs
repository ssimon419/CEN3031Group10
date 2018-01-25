using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanEnterPortal : MonoBehaviour {


    private CanEnterPortal other;


    public void detatch()
    {
        other.other = null;
    }

    public void updateOther(Portal enter, Portal exit) { 




        if (other == null)
        {

            Rigidbody2D bodyEnter = enter.GetComponent<Rigidbody2D>();
            Rigidbody2D bodyExit = exit.GetComponent<Rigidbody2D>();

            Rigidbody2D body = GetComponent<Rigidbody2D>();



            //   other = Instantiate(gameObject, new Vector3(body.position.x - bodyEnter.position.x + bodyExit.position.x, body.position.y - bodyEnter.position.y + bodyExit.position.y, 0), new Quaternion()).GetComponent<CanEnterPortal>();


            other = Instantiate(gameObject).GetComponent<CanEnterPortal>();

            Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), exit.on);
            

            Rigidbody2D bodyOther = other.GetComponent<Rigidbody2D>();

            other.other = this;
            bodyOther.position = new Vector3(body.position.x - bodyEnter.position.x + bodyExit.position.x, body.position.y - bodyEnter.position.y + bodyExit.position.y, 0);

            bodyOther.velocity = body.velocity;

        }





        //bodyOther.position = new Vector3(body.position.x - bodyEnter.position.x + bodyExit.position.x, body.position.y - bodyEnter.position.y + bodyExit.position.y, 0);


    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
