 #pragma strict

private var playerToTrack : GameObject;
private var blipMe : GameObject;

public var redBlipObject : GameObject;
//var blipLocationX : float;
//var blipLocationZ : float;
private var scale = 4062;
private var playerArray = new Array();

function Start () {
	playerToTrack = this.transform.parent.gameObject;
	blipMe = this.transform.Find('PlayerLocation').gameObject;
	InvokeRepeating("updatePosition", 1.0, 1.0); // only update the map once a second
	//InvokeRepeating("updateAll", 1.0, 1.0); // only update the map once a second
}

function removeRedBlips(){
	for(var redBlip : GameObject in GameObject.FindGameObjectsWithTag("temp"))
	{
	    Destroy(redBlip);
	}
}

function updatePlayersBlips() {
	for(var players : GameObject in GameObject.FindGameObjectsWithTag("NetPlayer"))
	{
	    // update each red blip location
		var redBlip : GameObject = Instantiate(redBlipObject);
		redBlip.transform.localPosition.x = players.transform.position.x / scale;
		redBlip.transform.localPosition.z = players.transform.position.z / scale;
		redBlip.transform.localPosition.y = 0.03;
		redBlip.transform.localRotation.y = players.transform.rotation.y;
		redBlip.tag = "temp";
	}
}

function updatePosition(){
	removeRedBlips();
	updatePlayersBlips();
	blipMe.transform.localPosition.x = playerToTrack.transform.position.x / scale;
	blipMe.transform.localPosition.z = playerToTrack.transform.position.z / scale;
	blipMe.transform.localPosition.y = 0.03;
	blipMe.transform.localRotation.y = playerToTrack.transform.rotation.y;
//	blipLocationX = blipMe.transform.localPosition.x;
//	blipLocationZ = blipMe.transform.localPosition.z;
}