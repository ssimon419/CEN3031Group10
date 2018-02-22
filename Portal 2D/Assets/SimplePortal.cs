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
      //  collider = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        
        //Code in here cane be used to destroy leftover clone
        
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //Make sure the object is allowed to go through the portal before we let it

        CanEnterPortal portalObject = other.GetComponent<CanEnterPortal>();

        if (portalObject != null && otherPortal != null)
        {
            //Teleport to other portal            


              //Position it correctly

                Rigidbody2D objectBody = portalObject.GetComponent<Rigidbody2D>();



                float relX = objectBody.position.x - body.position.x; //positive if to the right
                float relY = objectBody.position.y - body.position.y; //positive if above


                float a = body.rotation * Mathf.PI / 180;


                float dx = Mathf.Cos(a) * relX + Mathf.Sin(a) * relY;// + body.GetComponent<BoxCollider2D>().size.x * body.transform.localScale.x;
                float dy = Mathf.Cos(a) * relY + Mathf.Sin(a) * relX;

              //  dy *= -1;

                float a2 = otherPortal.body.rotation * Mathf.PI / 180;


                float x = otherPortal.body.position.x + Mathf.Cos(a2) * dx + Mathf.Sin(a2) * dy;
                float y = otherPortal.body.position.y + Mathf.Cos(a2) * dy + Mathf.Sin(a2) * dx;


                objectBody.position = new Vector3(x, y, 0);



                //TODO: Transfer velocity

                //TODO: Angels
                //Note that velocities are equal of portal is pointing opposite direction


            
            









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
