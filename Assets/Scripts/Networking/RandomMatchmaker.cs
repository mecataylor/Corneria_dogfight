using UnityEngine;
using System.Collections;

public class RandomMatchmaker : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings("0.1");
	}
	
	void OnGUI(){
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
	
	void OnJoinedLobby(){
		PhotonNetwork.JoinRandomRoom();
	}
	
	void OnPhotonRandomJoinFailed(){
		PhotonNetwork.CreateRoom(null);	
	}
	
	void OnJoinedRoom(){
		GameObject[] respawnLocations = GameObject.FindGameObjectsWithTag("Respawn");
		int count = respawnLocations.Length;
		int r = Random.Range(0, count);
		GameObject plane = PhotonNetwork.Instantiate("Player", respawnLocations[r].transform.position, Quaternion.identity, 0);
		MonoBehaviour planeControls = (plane.GetComponent("ShipBehavior") as MonoBehaviour);
		planeControls.enabled = true;
		MonoBehaviour boundries = (plane.GetComponent("KeepInBounds") as MonoBehaviour);
		boundries.enabled = true;

		//decide which camera
		Transform cameraTransform = plane.transform.FindChild("Main Camera");
		if(OVRDevice.IsHMDPresent()){
			cameraTransform = plane.transform.FindChild("OVRCameraController");
		}
		GameObject mainCamera = cameraTransform.gameObject;
		mainCamera.SetActive(true);
	}
}
