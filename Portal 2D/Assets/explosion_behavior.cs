using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion_behavior : MonoBehaviour {

	public GameObject parent;
	
	void finish_explosion(){
		Destroy (parent);
	}
}
