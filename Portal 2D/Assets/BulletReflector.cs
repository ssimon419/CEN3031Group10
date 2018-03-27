using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReflector : MonoBehaviour {

    private bool on;

    public bool isOn()
    {
        //return true;
        return on;
    }


	void Start () {
        on = false;
	}
	
	void FixedUpdate () {

        on = Input.GetKey(KeyCode.C);
        
        this.GetComponent<SpriteRenderer>().enabled = on;

    }
}
