#pragma strict

private var playerToTrack : GameObject;
private var blipMe : GameObject;
//var blipLocationX : float;
//var blipLocationZ : float;
private var scale = 4062;

function Start () {
	playerToTrack = this.transform.parent.gameObject;
	blipMe = this.transform.Find('PlayerLocation').gameObject;
	InvokeRepeating("updatePosition", 1.0, 1.0); // only update the map once a second
}

function updatePosition(){
	blipMe.transform.localPosition.x = playerToTrack.transform.position.x / scale;
	blipMe.transform.localPosition.z = playerToTrack.transform.position.z / scale;
	blipMe.transform.localPosition.y = 0.03;
	blipMe.transform.localRotation.y = playerToTrack.transform.rotation.y;
//	blipLocationX = blipMe.transform.localPosition.x;
//	blipLocationZ = blipMe.transform.localPosition.z;
}