    
var throttle : int = 60;
var boost : int = 10;
var xSensitivity : float = 1.0;
var rollSensitivity : float = 5.0;
var pitchSensitivity : float = 1.0;



function Update () {
    
	var forwardInput : float = Input.GetAxis("Triggers");
	var pitchInput : float = Input.GetAxis("LeftJoystickY");
	var rollInput : float = Input.GetAxis("RightJoystickX");
	var xInput : float = Input.GetAxis("LeftJoystickX");
	
	if(forwardInput < -.001){
		rigidbody.velocity = transform.forward * throttle * (boost * -forwardInput);
	}else{
		rigidbody.velocity = transform.forward * throttle;
	}
	
	var x : float = Time.deltaTime * throttle * xInput * xSensitivity;
	var roll : float = Time.deltaTime * throttle * rollInput * rollSensitivity;
	var pitch : float = Time.deltaTime * throttle * pitchInput * pitchSensitivity;
	
	var xRoll : float = 0;
	if(Mathf.Abs(x) > 0.05){
		xRoll = Mathf.Sign(x) * -1.5;
	}
	
	transform.Rotate(0, x, xRoll);
	transform.Rotate(-pitch, 0, 0);
	//for roll it might be better to Lerp to a fixed position based on where their joystick is
	// so for example, all the way to the right on the joystick is always turned 90 degrees right, no more
	transform.Rotate(0, 0, -roll);
     
	//self-righting
	//this needs some tweaking for when the ship is turned more than 180 degrees
	if(Mathf.Abs(transform.rotation.z) > .005){
		var right : float = Time.deltaTime * throttle * Mathf.Sign(transform.rotation.z);
		transform.Rotate(0, 0, right);
	}

}