using UnityEngine;
using System.Collections;

public class JoystickCamera : MonoBehaviour {

	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	float rotationX = 0F;
	float rotationY = 0F;
	
	Quaternion originalRotation;

	// Use this for initialization
	void Start () {
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
		originalRotation = transform.localRotation;	
	}
	
	// Update is called once per frame
	void Update () {
		// Read the right joystick input axis
		if(Env.OnAMac()){
			rotationX += Input.GetAxis("Triggers") * sensitivityX;
			rotationY += Input.GetAxis("RightJoystickX") * sensitivityY;
		}else{
			rotationX += Input.GetAxis("RightJoystickX") * sensitivityX;
			rotationY += Input.GetAxis("RightJoystickY") * sensitivityY;
		}
		
		rotationX = ClampAngle (rotationX, minimumX, maximumX);
		rotationY = ClampAngle (rotationY, minimumY, maximumY);
		
		Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
		
		transform.localRotation = originalRotation * xQuaternion * yQuaternion;	
	}

	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}
}
