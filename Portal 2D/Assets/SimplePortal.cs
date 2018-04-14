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

    private Transform body;
    private Transform trans;


    void Start()
    {
      //  collider = GetComponent<Collider2D>();
        body = GetComponent<Transform>();
        trans = GetComponent<Transform>();
		gameObject.GetComponent<BoxCollider2D> ().isTrigger = false;
		gameObject.GetComponent<BoxCollider2D> ().isTrigger = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {

        //Code in here cane be used to destroy leftover clone
        CanEnterPortal portalObject = other.GetComponent<CanEnterPortal>();

        if (portalObject != null)
            portalObject.exit(this);
        
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

            portalObject.enter(this);

              //Position it correctly

                Rigidbody2D objectBody = portalObject.GetComponent<Rigidbody2D>();

                float relX = objectBody.position.x - body.position.x; //positive if to the right
                float relY = objectBody.position.y - body.position.y; //positive if above


			float a = (body.rotation.eulerAngles.z + 90) * Mathf.PI / 180;
		
            

                float dx = Mathf.Cos(a) * relX + Mathf.Sin(a) * relY;// + body.GetComponent<BoxCollider2D>().size.x * body.transform.localScale.x;
                float dy = Mathf.Cos(a) * relY + Mathf.Sin(a) * relX;

                dy = 0;           
            
			float a2 = (otherPortal.body.rotation.eulerAngles.z+90) * Mathf.PI / 180;


           


                float x = otherPortal.body.position.x + Mathf.Cos(a2) * dx + Mathf.Sin(a2) * dy;
                float y = otherPortal.body.position.y + Mathf.Cos(a2) * dy + Mathf.Sin(a2) * dx;

                float temp = x - otherPortal.body.position.x;

            //super awkward way of fixing the x position

            if (relX * temp < 0)
            {
                temp *= -1;
                x = otherPortal.body.position.x + temp;
            }


            objectBody.position = new Vector3(x, y, 0);

           // objectBody.position = new Vector3(otherPortal.body.position.x, otherPortal.body.position.y, 0);


            //TODO: Transfer velocity


   


            float dvx = Mathf.Cos(a) * objectBody.velocity.x + Mathf.Sin(a) * objectBody.velocity.y;// + body.GetComponent<BoxCollider2D>().size.x * body.transform.localScale.x;
            float dvy = Mathf.Cos(a) * objectBody.velocity.y + Mathf.Sin(a) * objectBody.velocity.x;



            float vx = Mathf.Cos(a2) * dvx + Mathf.Sin(a2) * dvy;
            float vy = Mathf.Cos(a2) * dvy + Mathf.Sin(a2) * dvx;


            //new angle is difference + 180

            float vMag = Mathf.Sqrt( objectBody.velocity.x * objectBody.velocity.x + objectBody.velocity.y * objectBody.velocity.y);

  //          vMag *= 1.1f;

            float va = Mathf.Atan(objectBody.velocity.y / objectBody.velocity.x);

            if (objectBody.velocity.x < 0)
                va += Mathf.PI;


            float newVa = va + a2 - a + Mathf.PI;

            //   vMag = 0;


            newVa = a2;
            vMag = 20;


            objectBody.velocity = new Vector3(vMag * Mathf.Cos(newVa), vMag * Mathf.Sin(newVa), 0);

 //           objectBody.velocity = new Vector3(15, 15, 0);


            //TODO: Give a small boost in the direction of the normal


            //objectBody.velocity = new Vector3(vx, vy, 0);
            

        
        }
    }


    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
