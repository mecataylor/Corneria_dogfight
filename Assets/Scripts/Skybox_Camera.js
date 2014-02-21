#pragma strict
private var object : Transform;

function Update () {
	if ( object == null ){
		object = GameObject.Find("Main Camera").transform;
	}
	else {
		transform.rotation = object.rotation;
	}
}