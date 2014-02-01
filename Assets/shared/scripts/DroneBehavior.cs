using UnityEngine;
using System.Collections;

public class DroneBehavior : MonoBehaviour {

	public int networkID;
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
			SetLayerRecursively(newLaser.gameObject, Env.droneFireLayer);
			newLaser.AddForce(transform.forward * velocity, ForceMode.VelocityChange);		
		}
	}
	
	void netShieldUp(){
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
			//destroy laser
			Destroy (col.gameObject);
			gameObject.SendMessage("NetworkHit", networkID);
		}
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
		//death sequence
		Destroy(gameObject);
	}
}
