using UnityEngine;
using System.Collections;

public class ShipBehavior : MonoBehaviour {
	
	private GameObject shield;
	private bool shootOverride;
	private bool boosting;

	public static bool isShooting;
	public static bool shieldOn;

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
		shootOverride = false;
	}
	
	void Update () {
		if(Input.GetButtonDown("Jump")){
			shieldOn = true;
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
			isShooting = true;
			shootTheLazer();
		}else{
			isShooting = false;	
		}
		//shootOverride is used for network players
		if(shootOverride){
			shootTheLazer();
		}
	}
	
	void netShoot(){
		shootOverride = true;
	}
	
	void netStopShoot(){
		shootOverride = false;
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

	void netShieldUp(){
		shieldsUp();
	}
	
	void shieldsUp(){
		shield.SetActive(true);
		shieldOn = false;
	}

	void boostOn(){
		if(forwardInput() < -.001){
			boosting = true;
		}else{
			boosting = false;
		}
	}

	float forwardInput(){
		return Input.GetAxis("Triggers");
	}
	
	void fly() {

		float currentX = transform.rotation.x;
		float currentY = transform.rotation.y;
		float currentZ = transform.rotation.z;

		float pitchInput = Input.GetAxis("LeftJoystickY");
		float rollInput = Input.GetAxis("RightJoystickX");
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

}
