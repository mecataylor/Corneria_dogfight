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
			Rigidbody newLaser = Instantiate(bullet, transform.position, transform.rotation) as Rigidbody;
			newLaser.AddForce(transform.forward*velocity,ForceMode.VelocityChange);
		}
		
		
	}
}
