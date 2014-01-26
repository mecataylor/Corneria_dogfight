#pragma strict

function Start () {
	
	var measure = GetComponent(MeshFilter).mesh.bounds.size.y;
	Debug.Log( "Hello: " + measure );
	
}

function Update () {

}