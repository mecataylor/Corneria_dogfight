using UnityEngine;
using System.Collections;

public class PlayerFire : MonoBehaviour
{
	public Rigidbody bullet;
	public float velocity = 1250.0f;

	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			Vector3 shootVector = transform.position + new Vector3(1f, 0f, 1f);
			Rigidbody newLaser = Instantiate(bullet, shootVector, transform.rotation) as Rigidbody;
			newLaser.AddForce(transform.forward*velocity,ForceMode.VelocityChange);
		}
		
		
	}
}
