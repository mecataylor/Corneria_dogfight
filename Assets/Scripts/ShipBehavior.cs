using UnityEngine;
using System.Collections;

public class ShipBehavior : MonoBehaviour {
	
	private GameObject shield;
	private bool shootOverride;
	
	public Rigidbody bullet;
	public static bool isShooting;
	public float laser_velocity = 125.0f;
		
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
		fire ();
		shieldsUp();
		fly();
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
	
	void startShooting(){
		shootOverride = true;
	}
	
	void stopShooting(){
		shootOverride = false;
	}
	
	void shootTheLazer(){
		float velocity = laser_velocity + throttle;
		Rigidbody newLaser = Instantiate(bullet, transform.position, transform.rotation) as Rigidbody;
		newLaser.AddForce(transform.forward * velocity,ForceMode.VelocityChange);		
	}
	
	void shieldsUp(){
		if(Input.GetButtonDown("Jump")){
			shield.SetActive(true);	
		}		
	}
	
	void fly() {
		
		float forwardInput = Input.GetAxis("Triggers");
		float pitchInput = Input.GetAxis("LeftJoystickY");
		float rollInput = Input.GetAxis("RightJoystickX");
		float yawInput = Input.GetAxis("LeftJoystickX");
		
		if(forwardInput < -.001){
			rigidbody.velocity = transform.forward * throttle * (boost * -forwardInput);
		}else{
			rigidbody.velocity = transform.forward * throttle;
		}
		
		float yaw = Time.deltaTime * throttle * yawInput * xSensitivity;
		float roll = Time.deltaTime * throttle * rollInput * rollSensitivity;
		float pitch = Time.deltaTime * throttle * pitchInput * pitchSensitivity;

		//slight roll as you yaw
		float xRoll = 0;
		if(Mathf.Abs(yaw) > 0.05){
			xRoll = Mathf.Sign(yaw) * -rotateOnYaw;
			// This one might work for the railed version
			//transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, yaw, rotateOnYaw), Time.deltaTime * 1.5f);
		}
		
		transform.Rotate(0, yaw, xRoll);
		transform.Rotate(-pitch, 0, 0);
		transform.Rotate(0, 0, -roll);
		
		selfRighting();
	}
	
	void selfRighting(){
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * selfRightingSpeed);
	}
}
