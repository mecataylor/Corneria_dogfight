using UnityEngine;
using System.Collections;

public class ShipBehavior : MonoBehaviour {
	
	public Rigidbody laser;
	public Rigidbody missile;
	public Transform reticule;
	public Transform explosion;
	public float laser_velocity = 125.0f;
	public float fireRate = 0.75f;
	public float missileReloadTime = 3f;
	public float burstFireLength = 0.05f;
	public Transform[] cannons;
		
	public int throttle = 100;
	public int boost = 2;
	public float boostDuration = 2.0f;
	public float yawSensitivity = 1.0f;
	public float rollSensitivity = 5.0f;
	public float pitchSensitivity = 1.0f;
	public float selfRightingSpeed = 1.0f;

	private GameObject shield;
	private bool boosting;
	private bool firing;
	private bool missileReady = true;
	private float nextFire;
	private int phViewID;
	
	private string shootAxis = "Triggers";
	private string rollAxis = "RightJoystickX";
	private float inputThreshold = 0.001f;
	private int current_throttle = 0;

	void Start () {
		shield = transform.Find("shield").gameObject;

		//check for OSX for controls
		if(Env.OnAMac()){
			shootAxis = "MACTriggers";
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
		if(laserThreshold())
		{
			if(Time.time > nextFire){
				nextFire = Time.time + fireRate;
				firing = true;
			}
		}else{
			nextFire = Time.time;
		}

		if(missileThreshold()){
			if(missileReady){
				missileReady = false;
				fireTheMissile();
				StartCoroutine(LoadMissile());
			}
		}

		if(firing){
			shootTheLazer();
			gameObject.SendMessage("NetworkShoot");
			StartCoroutine(stopFiring());
		}
	}

	IEnumerator stopFiring(){
		yield return new WaitForSeconds(burstFireLength);
		firing = false;
	}

	IEnumerator LoadMissile(){
		yield return new WaitForSeconds(missileReloadTime);
		missileReady = true;
	}
	
	void shootTheLazer(){
		float velocityAdder = throttle;
		if(boosting){
			velocityAdder = throttle * boost;
		}
		float velocity = laser_velocity + velocityAdder;
		foreach(Transform cannon in cannons){
			Rigidbody newLaser = Instantiate(laser, cannon.position, transform.rotation) as Rigidbody;
			newLaser.AddForce(transform.forward * velocity, ForceMode.VelocityChange);
		}
	}

	void fireTheMissile(){
		Rigidbody newMissile = Instantiate(missile, cannons[0].position, transform.rotation) as Rigidbody;
		//This has weird results. We'll have to look at it more
		//newMissile.transform.LookAt(reticule);
		newMissile.gameObject.SendMessage("setVelocity", throttle);
		newMissile.AddForce(transform.forward, ForceMode.Impulse);
	}
	
	void shieldsUp(){
		shield.SetActive(true);
	}

	void boostOn(){
		if(Input.GetButtonDown("Fire1")){
			boosting = true;
			StartCoroutine(turnBoostOff());
		}
	}

	IEnumerator turnBoostOff(){
		yield return new WaitForSeconds(boostDuration);
		boosting = false;
	}

	float triggerInput(){
		return Input.GetAxis(shootAxis);
	}

	bool laserThreshold(){
		if(Env.OnAMac()){
			return triggerInput() > inputThreshold;
		}else{
			return triggerInput() < -inputThreshold;
		}
	}

	bool missileThreshold(){
		if(Env.OnAMac()){
			return triggerInput() < -inputThreshold;
		}else{
			return triggerInput() > inputThreshold;
		}
	}
	
	void fly() {

		setVelocity();

		setDirection();
	}

	void setVelocity(){
		//braking
		if(Input.GetButton("Fire2")){
			//slow down to stop
			current_throttle -= 10;
			if(current_throttle < 0){
				current_throttle = 0;
			}
		}else{
			//speed up to maximum
			current_throttle += 10;
			if(current_throttle > throttle){
				current_throttle = throttle;
			}
		}
		
		if(boosting){
			current_throttle *= boost;
		}

		if(boosting && Input.GetButton("Fire2")){
			current_throttle = throttle;
		}
		
		rigidbody.velocity = transform.forward * current_throttle;
	}

	void setDirection(){
		float pitchInput = Input.GetAxis("LeftJoystickY");
		float rollInput = Input.GetAxis(rollAxis);
		float yawInput = Input.GetAxis("LeftJoystickX");
		
		float yaw = Time.deltaTime * throttle * yawInput * yawSensitivity;
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
		if (this.enabled == false){
			return;
		}
		int layer = Env.enemyFireLayer;
		if(Env.isDogFight()){
			layer = Env.droneFireLayer;
		}
		if(col.gameObject.layer == layer){
			Destroy (col.gameObject);
			Instantiate(explosion, col.transform.position, col.transform.rotation);
			gameObject.SendMessage("hit");
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
		if (this.enabled == false){
			return;
		}
		PhotonNetwork.Destroy(gameObject);
		//death sequence
		Destroy(gameObject);
		//Application.Quit();
	}

}
