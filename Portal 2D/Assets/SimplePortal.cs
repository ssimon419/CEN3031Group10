using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePortal : MonoBehaviour
{

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


    public SimplePortal otherPortal; // The portal that this one links to. Note that otherPortal.otherPortal == this

    //private Collider2D collider;    //The portal's collider. Note that this collider might need to be offset so that you don't start going through the portal until you touch the wall its on


    //Components to prevent calling GetComponent<>() over and over

    private Rigidbody2D body;
    private Transform trans;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {

        //Make sure the object is allowed to go through the portal before we let it

        CanEnterPortal portalObject = other.GetComponent<CanEnterPortal>();

        if (portalObject != null && otherPortal != null)
        {

            if (!portalObject.allowEnter())
                return;

            //Teleport to other portal            

            portalObject.enter();

            //Position it correctly

            Rigidbody2D objectBody = portalObject.GetComponent<Rigidbody2D>();


            float a = (body.rotation + 90) * Mathf.PI / 180;
            float a2 = (otherPortal.body.rotation + 90) * Mathf.PI / 180;



            float relX = objectBody.position.x - body.position.x; //positive if to the right
            float relY = objectBody.position.y - body.position.y; //positive if above


           

            float dx = Mathf.Cos(a) * relX + Mathf.Sin(a) * relY;// + body.GetComponent<BoxCollider2D>().size.x * body.transform.localScale.x;
            float dy = Mathf.Cos(a) * relY + Mathf.Sin(a) * relX;

            dy = 0;






            float x = otherPortal.body.position.x + Mathf.Cos(a2) * dx + Mathf.Sin(a2) * dy;
            float y = otherPortal.body.position.y + Mathf.Cos(a2) * dy + Mathf.Sin(a2) * dx;

            float temp = x - otherPortal.body.position.x;

            //super awkward way of fixing the x position: (Fix this)

            if (relX * temp < 0)
            {
                temp *= -1;
                x = otherPortal.body.position.x + temp;
            }
           

            //Works cleaner if the player comes out centered

            objectBody.position = new Vector3(x, y, 0);

            // objectBody.position = new Vector3(otherPortal.body.position.x, otherPortal.body.position.y, 0);


            //  Transfer velocity

            
            //Get angle and magniture of object's velocity
 
            float vMag = Mathf.Sqrt(objectBody.velocity.x * objectBody.velocity.x + objectBody.velocity.y * objectBody.velocity.y);


            float va = Mathf.Atan(objectBody.velocity.y / objectBody.velocity.x);

            if (objectBody.velocity.x < 0)
                va += Mathf.PI;


            //After many experiments, turns out its clear to just shoot out in the direction of the portal

            float newVa = a2;
            
            vMag = vMag*Mathf.Cos(va - a); //The velocity with respect to the normal of the entrance portal

            if (vMag < 0)
                vMag *= -1;

            if (vMag < 10)
                vMag = 10;

 
            objectBody.velocity = new Vector3(vMag * Mathf.Cos(newVa), vMag * Mathf.Sin(newVa), 0);

 

        }
    }


    void FixedUpdate()
    {
        
    }

    void Update()
    {

    }
}
