using UnityEngine;
using System.Collections;

public class DroneBehavior : MonoBehaviour {

	public int networkID;
	private GameObject shield;

	public Rigidbody bullet;
	public Rigidbody missile;
	public Transform explosion;
	public float laser_velocity = 125.0f;
	public Transform[] cannons;
	public int throttle = 60;

	// Use this for initialization
	void Start () {
		shield = transform.Find("shield").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Shoot(){
		if (this.enabled == false){
			return;
		}
		float velocity = laser_velocity + throttle;
		foreach(Transform cannon in cannons){
			Rigidbody newLaser = Instantiate(bullet, cannon.position, transform.rotation) as Rigidbody;
			SetLayerRecursively(newLaser.gameObject, Env.droneFireLayer);
			newLaser.AddForce(transform.forward * velocity, ForceMode.VelocityChange);		
		}
	}

	void MissileShoot(){
		if (this.enabled == false){
			return;
		}
		Rigidbody newMissile = Instantiate(missile, cannons[0].position, transform.rotation) as Rigidbody;
		SetLayerRecursively(newMissile.gameObject, Env.droneFireLayer);
		//This has weird results. We'll have to look at it more
		//newMissile.transform.LookAt(reticule);
		newMissile.gameObject.SendMessage("setVelocity", throttle);
		newMissile.AddForce(transform.forward, ForceMode.Impulse);
	}
	
	void shieldUp(){
		if (this.enabled == false){
			return;
		}
		shield.SetActive(true);
	}
	
	void myNetViewID(int id){
		networkID = id;
	}

	void OnCollisionEnter (Collision col)
	{
		if (this.enabled == false){
			return;
		}
		if(col.gameObject.layer == Env.playerFireLayer){
			takeAhit(col.gameObject);
		}
	}

	void takeAhit(GameObject bullet)
	{
		Destroy (bullet);
		Instantiate(explosion, bullet.transform.position, bullet.transform.rotation);
		gameObject.SendMessage("NetworkHit", networkID);
	}

	void SetLayerRecursively(GameObject go, int layerNumber){
		foreach (Transform trans in go.GetComponentsInChildren<Transform>(true)){
			trans.gameObject.layer = layerNumber;
		}
	}

	void healed(){
		
	}
	
	void damaged(){
		
	}
	
	void dead(){
		if (this.enabled == false){
			return;
		}
		//death sequence
	}
}
