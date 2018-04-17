using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformmovement : MonoBehaviour {
    private Vector3 posA;
    private Vector3 posB;
    private Vector3 nexpos;
    [SerializeField] private float amount;
    [SerializeField] private Transform childtransform;
    [SerializeField] private Transform transB;
	[SerializeField] private bool repeat;
	[SerializeField] private bool stop;

	private bool rev=false;

    private Vector2 dir;
	// Use this for initialization
	void Start () {
        dir = new Ray2D(childtransform.position, transB.position - childtransform.position).direction.normalized;
		posB = transB.position;
		posA = childtransform.position;
        dir *= amount;
	}
	
	// Update is called once per frame
	void Update () {
        move();
		if (Vector2.Distance (childtransform.position, posB) <= 0.1f && repeat) {
			if (!rev) {
				dir *= -1;
				posB = posA;
				rev = true;
			} else {
				dir *= -1;
				posB = transB.position;
				rev = false;
			}
		}
		else if (Vector2.Distance (childtransform.position, posB) <= 0.1f && stop)
			gameObject.SetActive (false);
	}

    private void move()
    {
        childtransform.Translate(dir.x, dir.y,0f);
    }
}
