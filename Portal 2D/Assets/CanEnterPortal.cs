using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanEnterPortal : MonoBehaviour {


    /*
     * Attach this script to a gameobject to allow it to enter a portal
     * 
     * Note that when your object goes through a portal, it is duplicated and potentially destroyed.
     *  Make sure your code does not rely on waiting for objects to be created / destroyed
     *  if that object can enter a portal
     * 
     * 
     * 
     * This script handles the duplication, etc
     * It also contains a reference to the duplicate, if it exists
     
     */



    private CanEnterPortal other; //the duplicate. Note that other.other == this

    private bool goingThroughPortal = false;


    bool allowEnter()
    {
        return !goingThroughPortal;
    }

    public void enter() {
        goingThroughPortal = true;
    }

    public void exit()
    {
        goingThroughPortal = false;
    }

    public void destroyOther()
    {
        if(other != null)
         Destroy(other.gameObject);
    }

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


            //Duplicate self

            other = Instantiate(gameObject).GetComponent<CanEnterPortal>();

            //Ignore collisions with the other wall

            Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), exit.on);
            

            Rigidbody2D bodyOther = other.GetComponent<Rigidbody2D>();

            other.other = this;
            

            //Position it correctly

            //float dx = bodyExit.GetComponent<Collider2D>().bounds.size.x;
            
            
            //TODO: Use angle instead

            if (exit.isLeft)
            {
              //  dx *= -1;
            }


            float relX = body.position.x - bodyEnter.position.x; //positive if to the right
            float relY = body.position.y - bodyEnter.position.y; //positive if above


            float a = bodyEnter.rotation * Mathf.PI/180;


            float dx = Mathf.Cos(a) * relX + Mathf.Sin(a) * relY + bodyEnter.GetComponent<BoxCollider2D>().size.x * bodyEnter.transform.localScale.x; 
            float dy = Mathf.Cos(a) * relY + Mathf.Sin(a) * relX;
            

            float a2 = bodyEnter.rotation * Mathf.PI / 180;


            float x = bodyExit.position.x + Mathf.Cos(a) * dx + Mathf.Sin(a) * dy;
            float y = bodyExit.position.y + Mathf.Cos(a) * dy + Mathf.Sin(a) * dx;


            bodyOther.position = new Vector3(x, y, 0);



            //Transfer velocity

            //TODO: Angels
            //Note that velocities are equal of portal is pointing opposite direction


            bodyOther.velocity = body.velocity;

        }




    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
