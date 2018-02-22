using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    /*
     * 
     * Rotation for portals determines where the front end of the portal is (the end you enter and exit)
     *      0 degrees points right
     *      90 degress points up (portal on ground)
     *      180 degress points left
     *      270 degrees points down (portal on ceiling)
     *
     *
     * 
     * 
     */

        
    public Portal otherPortal; // The portal that this one links to. Note that otherPortal.otherPortal == this

    public Collider2D on;   //The wall that the portal is on. Collision needs to be disabled while we are going through the portal

    private Collider2D collider;    //The portal's collider. Note that this collider is offset so that you don't start going through the portal until you touch the wall its on

    public Rigidbody2D portalEdgePrefab; //Used to create two small colliders on either end of the portal. Acts as temporary side walls for the portal

    //instances of the portal edges

    private Rigidbody2D top;    
    private Rigidbody2D bottom;


    //Components to prevent calling GetComponent<>() over and over

    private Rigidbody2D body;
    private Transform trans;


    //TODO: Use rotation instead

    public bool isLeft;
    

	void Start () {
        collider = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();

    }

    void OnTriggerExit2D(Collider2D other)
    {
        

        CanEnterPortal portalObject = other.GetComponent<CanEnterPortal>();

        

        if (portalObject != null)
        {
            Destroy(top.gameObject);
            Destroy(bottom.gameObject);

            Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), on, false);


            //TODO: Make this work with angles

            /*
             * Pseudocode
             * 
             * Get angle between center of portal and center of object
             * If this angle between angle of portal +-90, we are good to go
             * 
             * 
             */

            Rigidbody2D otherBody = other.GetComponent<Rigidbody2D>();

            float dx = otherBody.position.x - body.position.x;
            float dy = otherBody.position.y - body.position.y;

            float a = body.rotation;    // in degrees
            float a0 = a - 90;
            float a2 = a + 90;


            float a1 = Mathf.Atan(dy / dx) * 180 / Mathf.PI;            //from -90 to + 90

            if (dx < 0)
                a1 += 180;

            if (a1 < 0)
                a1 += 360;

            if (a < 0)
                a += 360;


            if(a1 - a > -90 && a1 - a < 90)
            {
                portalObject.destroyOther();
            }
            else {
                portalObject.detatch();
                Destroy(portalObject.gameObject);
            }




        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        //Make sure the object is allowed to go through the portal before we let it

        CanEnterPortal portalObject = other.GetComponent<CanEnterPortal>();

        if (portalObject != null)
        {

            //Make the portal edges and position them correctly

            top    = Instantiate(portalEdgePrefab.gameObject).GetComponent<Rigidbody2D>();
            bottom = Instantiate(portalEdgePrefab.gameObject).GetComponent<Rigidbody2D>();

            float dx = GetComponent<BoxCollider2D>().size.x * trans.localScale.x / 2 + top.GetComponent<Collider2D>().bounds.size.x / 2;

            dx *= -1;


            float dy = GetComponent<BoxCollider2D>().size.y * trans.localScale.y / 2 + top.GetComponent<Collider2D>().bounds.size.y;




            float a = body.rotation * Mathf.PI / 180;




            top.GetComponent<Transform>().position = new Vector3(body.position.x + Mathf.Cos(a) * dx + Mathf.Sin(a) * dy,
                                                                 body.position.y + Mathf.Cos(a) * dy + Mathf.Sin(a) * dx, 0);


            bottom.GetComponent<Transform>().position = new Vector3(body.position.x + Mathf.Cos(a) *  dx + Mathf.Sin(a) * -dy,
                                                                 body.position.y + Mathf.Cos(a) * -dy + Mathf.Sin(a) *  dx, 0);
            
            top.rotation = body.rotation;
            bottom.rotation = body.rotation;


            //Turn off collision to allow it through

            Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), on, true);


            //Duplicate the object going through the portal, so that it will appear at both ends

            portalObject.updateOther(this, otherPortal);

        }
    }

    
    void FixedUpdate()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
