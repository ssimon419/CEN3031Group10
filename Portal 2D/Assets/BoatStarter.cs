using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatStarter : MonoBehaviour {

    public boatScript boat;


 
    void OnTriggerExit2D(Collider2D other)
    {
        boat.beginMoving();

    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
