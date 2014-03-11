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

function syncRedBlips(){
	var netPlayers = GameObject.FindGameObjectsWithTag("NetPlayer");
	var netC = netPlayers.length;
	var redBlips = GameObject.FindGameObjectsWithTag("temp");
	var difference = netC - redBlips.length;
//	Debug.Log("Difference: " + difference);
	if (difference > 0){
		// add blips
		for(var i=0;i<difference;i++){
			var redBlip : GameObject = Instantiate(redBlipObject);
			redBlip.transform.parent = this.transform;
			redBlip.transform.localRotation.y = 0.0;
			redBlip.transform.localRotation.x = 0.0;
			redBlip.transform.localRotation.z = 0.0;
			redBlip.tag = "temp";
		}
	}
	else if (difference < 0){
		//remove blips
		var j = 0;
		for(var redBlip2 : GameObject in GameObject.FindGameObjectsWithTag("temp"))
		{
		    if ( j > difference ){
		    	Destroy(redBlip2);
		    	j--;
		    }
		}
	}
//	for(var redBlip : GameObject in GameObject.FindGameObjectsWithTag("temp"))
//	{
//	    Destroy(redBlip);
//	}
}

function updatePlayersBlips() {
	var redBlips = GameObject.FindGameObjectsWithTag("temp");
	var players = GameObject.FindGameObjectsWithTag("NetPlayer");
	var k = 0;
	for(var player : GameObject in players)
	{
	    // update each red blip location
		var redBlip : GameObject = redBlips[k];
//		redBlip.transform.parent = this.transform;
		redBlip.transform.localPosition.x = player.transform.position.x / scale;
		redBlip.transform.localPosition.z = player.transform.position.z / scale;
		redBlip.transform.localPosition.y = 0.03;
		redBlip.transform.localRotation.y = player.transform.rotation.y;
//		redBlip.tag = "temp";
		k++;
	}
}

function updatePosition(){
	syncRedBlips();
	updatePlayersBlips();
	blipMe.transform.localPosition.x = playerToTrack.transform.position.x / scale;
	blipMe.transform.localPosition.z = playerToTrack.transform.position.z / scale;
	blipMe.transform.localPosition.y = 0.03;
	blipMe.transform.localRotation.y = playerToTrack.transform.rotation.y;
//	blipLocationX = blipMe.transform.localPosition.x;
//	blipLocationZ = blipMe.transform.localPosition.z;
}