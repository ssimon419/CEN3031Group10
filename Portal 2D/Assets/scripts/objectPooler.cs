using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPooler : MonoBehaviour {

	//GENERIC POOLER CLASS - can pool ANY object wow : )

	public static objectPooler current; //this current pooler
	public GameObject pooledObj; //object to be pooled
	public int pooledAmt = 20; //how many objects to be pooled
	public bool willGrow = true; //if there aren't enough objects, expand the list?

	List<GameObject> pooledObjs;
	//!!WILL NOT SHRINK!!

	void Awake(){
		current = this;
	}

	void Start () {
		pooledObjs = new List<GameObject> ();
		//fill list
		for (int i = 0; i < pooledAmt; i++) {
			GameObject obj = (GameObject)Instantiate (pooledObj,gameObject.transform);
			obj.SetActive (false);
			pooledObjs.Add (obj);
		}
	}

	public GameObject GetPooledObject(){
		for (int i = 0; i < pooledObjs.Count; i++) {
			if (!pooledObjs [i].activeInHierarchy) {
				return pooledObjs [i];
			}
		}

		if (willGrow) {
			GameObject obj = (GameObject)Instantiate (pooledObj,gameObject.transform);
			pooledObjs.Add (obj);
			return obj;
		}

		return null;
	}

	//TO ACCESS:
	/*
	 * 
	 * 
	GameObject obj = poolManager.pools[i].GetPooledObject(); //gives object or null
	if(obj == null) return; //make sure u dont heck up!!!!!!!!!!!!!!!
	obj.Initialize(parameters); //start up the object, make sure you have a public Initialize
	obj.SetActive(true);
	*/
}
