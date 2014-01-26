using UnityEngine;
using System.Collections;

public class ShipBehavior : MonoBehaviour {
	
	private GameObject shield;
	private bool boosting;
	private int phViewID;

	private string boostAxis = "Triggers";
	private string rollAxis = "RightJoystickX";

	public Rigidbody bullet;
	public float laser_velocity = 125.0f;
	public Transform[] cannons;
		
	public int throttle = 60;
	public int boost = 10;
	public float xSensitivity = 1.0f;
	public float rollSensitivity = 5.0f;
	public float pitchSensitivity = 1.0f;
	public float selfRightingSpeed = 1.0f;
	public float rotateOnYaw = 1f;

	void Start () {
		shield = transform.Find("shield").gameObject;

		//check for OSX for controls
		if(Env.OnAMac()){
			boostAxis = "MACTriggers";
			rollAxis = "Triggers";
		}
	}
	
	void Update () {
		if(Input.GetButtonDown("Jump")){
			gameObject.SendMessage("NetworkShield");
			shieldsUp();
		}
		boostOn();
		fly();
		fire();
	}
	
	//fire when ready
	void fire(){
		if(Input.GetButtonDown("Fire1"))
		{
			shootTheLazer();
			gameObject.SendMessage("NetworkShoot");
		}
	}
	
	void shootTheLazer(){
		float velocityAdder = throttle;
		if(boosting){
			velocityAdder = throttle * boost;
		}
		float velocity = laser_velocity + velocityAdder;
		foreach(Transform cannon in cannons){
			Rigidbody newLaser = Instantiate(bullet, cannon.position, transform.rotation) as Rigidbody;
			newLaser.AddForce(transform.forward * velocity, ForceMode.VelocityChange);		
		}
	}
	
	void shieldsUp(){
		shield.SetActive(true);
	}

	void boostOn(){
		if(forwardInput() < -.001){
			boosting = true;
		}else{
			boosting = false;
		}
	}

	float forwardInput(){
		return Input.GetAxis(boostAxis);
	}
	
	void fly() {

		float pitchInput = Input.GetAxis("LeftJoystickY");
		float rollInput = Input.GetAxis(rollAxis);
		float yawInput = Input.GetAxis("LeftJoystickX");
		
		if(boosting){
			rigidbody.velocity = transform.forward * throttle * (boost * -forwardInput());
		}else{
			rigidbody.velocity = transform.forward * throttle;
		}
		
		float yaw = Time.deltaTime * throttle * yawInput * xSensitivity;
		float roll = Time.deltaTime * throttle * rollInput * rollSensitivity;
		float pitch = Time.deltaTime * throttle * pitchInput * pitchSensitivity;

		transform.Rotate(-pitch, yaw, -roll);

		//if you're not rolling it will self-right
		if(rollInput == 0){
			selfRighting();
		}
	}
	
	void selfRighting(){
		Vector3 flatFwd = new Vector3(transform.forward.x, 0, transform.forward.z);
		Quaternion fwdRotation = Quaternion.LookRotation(flatFwd, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, fwdRotation, Time.deltaTime * selfRightingSpeed );
	}


	void OnCollisionEnter (Collision col)
	{
		int layer = Env.enemyFireLayer;
		if(Env.isDogFight()){
			layer = Env.droneFireLayer;
		}
		if(col.gameObject.layer == layer){
			gameObject.SendMessage("damage", Env.laserDamageAmount);
		}
	}

	void myNetViewID(int id){
		phViewID = id;
	}

	void healed(){

	}

	void damaged(){

	}

	void dead(){
		Debug.Log ("I'm dead, Jim");
		PhotonNetwork.Destroy(gameObject);
		Destroy(gameObject);
		//Application.Quit();
	}

}
