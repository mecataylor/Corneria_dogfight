    
var throttle : int;                 // current value for throttle
var maxThrottle : int;              // maximum value for throttle
var minThrottle : int;              // minimum value for throttle
var alieronSensitivity : float;     // how sensitive the ship is to rotation (could be adjusted according to speed)
var selfRightingSpeed : float;      // damping speed for alieron rotation
var elevatorSensitivity : float;    // how sensitive the ship is to up and down rotation (could be adjusted according to speed)
var tailResponsivness : float;
var rudderSensitivity : float;

function Start () {

}

function controlThrottle(){
    // throttle controls
    if (Input.GetButtonDown ("RightBumper")) {
		throttle += 5;
        if ( throttle > maxThrottle ){
            throttle = maxThrottle;
        }
	}
    if (Input.GetButtonDown ("LeftBumper")) {
        throttle -= 5;
        if ( throttle < minThrottle ){
            throttle = minThrottle;
        }
    }
}

function controlAlierons(){
    // set the percent throttle for making the plane more or less responsive at different speeds.
    var throttlePercent = ((rigidbody.velocity.magnitude * 1.0) / maxThrottle);
    
    // alieron rotation
    if ( Mathf.Abs( Input.GetAxis ( "RightJoystickX" ) ) > 0.15 ) {
        rigidbody.AddRelativeTorque( Vector3.forward * -1 * Input.GetAxis ( "RightJoystickX" ) * alieronSensitivity * throttlePercent );
    }
    else {
        rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, Vector3.zero, Time.deltaTime * selfRightingSpeed);
    }
}

function controlElevators(){
    // elevator rotation
    if ( Mathf.Abs( Input.GetAxis ( "RightJoystickY" ) ) > 0.15 ) {
        rigidbody.AddRelativeTorque( Vector3.left * Input.GetAxis ( "RightJoystickY" ) * elevatorSensitivity );
    }
    else {
        rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, Vector3.zero, Time.deltaTime * tailResponsivness);
    }
}

function controlRudder(){
	// elevator rotation
    if ( Mathf.Abs( Input.GetAxis ( "LeftJoystickX" ) ) > 0.15 ) {
        rigidbody.AddRelativeTorque( Vector3.up * Input.GetAxis ( "LeftJoystickX" ) * rudderSensitivity );
    }
    else {
        rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, Vector3.zero, Time.deltaTime * tailResponsivness);
    }
}

function calculateLift(){
	rigidbody.AddRelativeForce( Vector3.up * rigidbody.velocity.magnitude * 0.25 );
}

function Update () {
    controlThrottle();
    controlAlierons();
    controlElevators();
    calculateLift();
    controlRudder();
    
    
    rigidbody.AddRelativeForce( Vector3.forward*throttle );
    
    
}