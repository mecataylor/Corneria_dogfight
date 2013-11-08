#pragma strict

public var duration : float = 3f;

private var target : GameObject = null;
private var turnSpeed : float = 1000;

function Start(){
	Destroy (gameObject, duration);
}

function Update () {
	transform.Rotate(Vector3.up + Vector3.right, turnSpeed * Time.deltaTime);
	if(target){
		transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime);
	}
}

function setTarget(toTarget : GameObject){
	target = toTarget;
}