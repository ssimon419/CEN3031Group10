using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {


    public Portal otherPortal;

    public Collider2D on;

    private Collider2D collider;

    public bool isLeft;
    

	void Start () {
        collider = GetComponent<Collider2D>();
        
	}

    void OnTriggerExit2D(Collider2D other)
    {
        CanEnterPortal portalObject = other.GetComponent<CanEnterPortal>();

        if (portalObject != null)
        {
            Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), on, false);

            if (isLeft && portalObject.GetComponent<Rigidbody2D>().position.x > this.GetComponent<Rigidbody2D>().position.x)
            {
                portalObject.detatch();
                Destroy(portalObject.gameObject);

            } else if (!isLeft && portalObject.GetComponent<Rigidbody2D>().position.x < this.GetComponent<Rigidbody2D>().position.x)
            
            {
                portalObject.detatch();
                Destroy(portalObject.gameObject);
            }

            

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        CanEnterPortal portalObject = other.GetComponent<CanEnterPortal>();

        if (portalObject != null)
        {

            

            portalObject.updateOther(this, otherPortal);

            Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), on, true);


            //Rigidbody2D body = other.GetComponent<Rigidbody2D>();// = new Vector3(10,10,0);

           // Rigidbody2D bodyP = otherPortal.GetComponent<Rigidbody2D>();// = new Vector3(10,10,0);
            
            //body.position = new Vector3(bodyP.position.x, bodyP.position.y + 5, 0);
        }
    }

    
    void FixedUpdate()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
