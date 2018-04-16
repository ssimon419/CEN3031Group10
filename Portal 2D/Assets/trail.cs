using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class trail : MonoBehaviour
{

    private portal_gun pg;

	public LayerMask whatToHit;

    public SpriteRenderer spr;
    private Ray2D r2d;
    private float speed;
	private Transform firepoint;
    private Rigidbody2D rb2d;


    public Transform portal;


	public void Initialize(Ray2D r, Transform fp, Transform p, float s)
    {
		firepoint = fp;
		rb2d = GetComponent<Rigidbody2D> ();
		speed = s;
		r2d = r;
		transform.rotation = Quaternion.LookRotation (Vector3.forward, r2d.direction);
        portal = p;
		fireBullet ();
    }

	public void fireBullet(){
		rb2d.velocity = r2d.direction * speed;
	}

    // Use this for initialization lol
   

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("ground"))
        {
			portal.gameObject.SetActive (true);
			RaycastHit2D hit = Physics2D.Raycast(firepoint.position, transform.position-firepoint.position, 100,whatToHit);
			Debug.DrawRay (hit.point, hit.normal,Color.black);
			portal.SetParent(other.transform); //assigns portals to the objects they hit so that they move relative to these objects
			portal.localPosition = other.transform.InverseTransformPoint(hit.point);
			portal.rotation = Quaternion.LookRotation (new Vector3 (0.0f, 0.0f, 1f), hit.normal);	
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("environment"))
        {
            Destroy(gameObject);
        }
    }
}