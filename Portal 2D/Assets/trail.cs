using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class trail : MonoBehaviour
{

    private portal_gun pg;

    public SpriteRenderer spr;
    private Ray2D r2d;
    private float speed;
    private Vector2 placement;
    private Vector2 normal;
    private Rigidbody2D rb2d;


    public Transform portal;


	public void Initialize(Ray2D r, Vector3 location, Vector3 n, Transform p, float s)
    {
		rb2d = GetComponent<Rigidbody2D> ();
		speed = s;
		r2d = r;
        portal = p;
        placement = location;
        normal = n;
		fireBullet ();
    }

	public void fireBullet(){
		rb2d.velocity = r2d.direction * speed;
		Debug.Log ("fired bullet");
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
            Destroy(gameObject);
			portal.gameObject.SetActive (true);
			portal.SetParent(other.transform); //assigns portals to the objects they hit so that they move relative to these objects
			portal.localPosition = other.transform.InverseTransformPoint(placement);
			portal.rotation = Quaternion.LookRotation (new Vector3 (0.0f, 0.0f, 1f), normal);	
        }
        else if (other.gameObject.CompareTag("environment"))
        {
            Destroy(gameObject);
        }
    }
}