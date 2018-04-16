using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion_behavior : MonoBehaviour {

	public GameObject parent;

	void OnEnable(){
		transform.rotation = Quaternion.identity;
	}
	
	void finish_explosion(){
		parent.GetComponent<MonoBehaviour> ().CancelInvoke ();
		parent.SetActive (false);
		gameObject.SetActive (false);
	}
}
