using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal_gun : MonoBehaviour {

	public float fireRate = 0;
	public LayerMask whatToHit;

	public Transform portal1;
	public Transform portal2;
	public Transform BulletTrailPrefab;

	public float effectSpawnRate = 10;
	float timeToSpawnEffect = 0;
	float timeToFire = 0;
	Transform firePoint;

	// Use this for initialization
	void Awake  () {
		firePoint = transform.Find("FirePoint");
		if (firePoint == null)
		{
			Debug.LogError("No firepoint?");
		}
	}

	float Angle2(Vector2 a, Vector2 b){
		return Mathf.Atan2 (a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}

	// Update is called once per frame
	void Update () {
		//  Shoot();		
		Vector2 posOnScreen=transform.position;
		Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
		float angle = Angle2(posOnScreen, mouseOnScreen);
		transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, angle));

		if (fireRate == 0)
		{
			if (Input.GetButtonDown("Fire1"))
			{
				Shoot(true);
			}
			else if (Input.GetButtonDown("Fire2"))
			{
				Shoot(false);
			}
		}
		else
		{
			if (Input.GetButton("Fire1") && Time.time > timeToFire)
			{
				timeToFire = Time.time + 1 / fireRate;
				Shoot(true);
			}
			else if (Input.GetButton("Fire2") && Time.time > timeToFire)
			{
				timeToFire = Time.time + 1 / fireRate;
				Shoot(false);
			}
		}
	}

	void Shoot (bool a)
	{
		Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
		if (Time.time >= timeToSpawnEffect)
		{
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}
		Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition)*100);


		if (hit.collider != null)
		{
			Debug.DrawLine(firePointPosition, hit.point, Color.red);
			Debug.Log("We hit" + hit.collider.name);
			Debug.DrawRay (hit.point, hit.normal*3);
			if (a) { 
				//square raycast from hit point?
				portal1.SetParent(hit.transform); //assigns portals to the objects they hit so that they move relative to these objects
				portal1.localPosition = hit.transform.InverseTransformPoint(hit.point);
				portal1.rotation = Quaternion.LookRotation (new Vector3 (0.0f, 0.0f, 1f), hit.normal);	
			} else {
				portal2.SetParent(hit.transform); //acts a little goofy though, will need fixing
				portal2.localPosition = hit.transform.InverseTransformPoint(hit.point);
				portal2.rotation = Quaternion.LookRotation (new Vector3 (0.0f, 0.0f, 1f), hit.normal);
			}
		}
	}

	/*void Shoot2()
	{
		Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
		if (Time.time >= timeToSpawnEffect)
		{
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}
		Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100);

		if (hit.collider != null)
		{
			Debug.DrawLine(firePointPosition, hit.point, Color.red);
			Debug.Log("We hit" + hit.collider.name);
			Debug.DrawRay (hit.point, hit.normal*3);
			portal2.position = hit.point;
		}
	}*/
}