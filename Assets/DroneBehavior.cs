using UnityEngine;
using System.Collections;

public class DroneBehavior : MonoBehaviour {

	private int phViewID;
	private GameObject shield;

	public Rigidbody bullet;
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

	void netShoot(){
		float velocity = laser_velocity + throttle;
		foreach(Transform cannon in cannons){
			Rigidbody newLaser = Instantiate(bullet, cannon.position, transform.rotation) as Rigidbody;
			newLaser.AddForce(transform.forward * velocity, ForceMode.VelocityChange);		
		}
	}
	
	void netShieldUp(){
		shield.SetActive(true);
	}
	
	void myNetViewID(int id){
		phViewID = id;
	}

	void OnCollisionEnter (Collision col)
	{
		int layer = Env.enemyFireLayer;
		if(Env.isDogFight()){
			layer = Env.playerFireLayer;
		}
		if(col.gameObject.layer == layer){
			transform.SendMessage("NetworkHit", phViewID);
		}
	}
	
	void netDied(){
		//death sequence
		Debug.Log (phViewID + ": I died");
	}
}
