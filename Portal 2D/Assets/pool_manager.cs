using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pool_manager : MonoBehaviour {

	public objectPooler[] pools;

	public static List<objectPooler> heldPools;

	void Awake(){
		heldPools = new List<objectPooler> ();
		for(int i=0; i<pools.Length;++i){
			heldPools.Add(pools[i]);
		}
	}
}
