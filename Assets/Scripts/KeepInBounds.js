#pragma strict

var mapXMax : int;
var mapXMin : int;
var mapYMax : int;
var mapYMin : int;
var mapZMax : int;
var mapZMin : int;

function Start () {

}

function keepInBounds(){
	if (rigidbody.transform.position.x < mapXMin ){
		rigidbody.transform.position.x = mapXMax;
	}
	if (rigidbody.transform.position.x > mapXMax ){
		rigidbody.transform.position.x = mapXMin;
	}
	if (rigidbody.transform.position.y < mapYMin ){
		rigidbody.transform.position.y = mapYMin;
	}
	if (rigidbody.transform.position.y > mapYMax ){
		rigidbody.transform.position.y = mapYMax;
	}
	if (rigidbody.transform.position.z < mapZMin ){
		rigidbody.transform.position.z = mapZMax;
	}
	if (rigidbody.transform.position.z > mapZMax ){
		rigidbody.transform.position.z = mapZMin;
	}
}

function Update () {
	keepInBounds();
}