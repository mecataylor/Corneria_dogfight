﻿using UnityEngine;
using System.Collections;

public class RandomMatchmaker : MonoBehaviour {

	public bool rift = true;

	private GameObject[] respawnLocations;

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings("0.1");
		respawnLocations = GameObject.FindGameObjectsWithTag("Respawn");
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
		int count = respawnLocations.Length;
		int r = Random.Range(0, count);
		GameObject plane = PhotonNetwork.Instantiate("Player", respawnLocations[r].transform.position, Quaternion.identity, 0);
		initScripts(plane);
		SetLayerRecursively(plane, Env.playerLayer);

		//activate the radar for local player only
		GameObject Radar_Map = plane.transform.FindChild ("Radar_Map").gameObject;
		Radar_Map.SetActive (true);

		// manage the cameras
		if (rift) {
			//detect Rift
			Transform riftCameraTransform = plane.transform.FindChild ("OVRCameraController");
			GameObject riftCamera = riftCameraTransform.gameObject;
			riftCamera.SetActive (true);

			//decide which camera
			if (!OVRDevice.IsHMDPresent ()) {
					//first deactivate Rift camera
					riftCamera.SetActive (false);
					//activate normal camera
					Transform cameraTransform = plane.transform.FindChild ("Main Camera");
					GameObject mainCamera = cameraTransform.gameObject;
					mainCamera.SetActive (true);
			}
		}
		else {
			//activate normal camera
			Transform cameraTransform = plane.transform.FindChild ("Main Camera");
			GameObject mainCamera = cameraTransform.gameObject;
			mainCamera.SetActive (true);
		}

	}

	void initScripts(GameObject plane){
		MonoBehaviour drone = (plane.GetComponent("DroneBehavior") as MonoBehaviour);
		drone.enabled = false;
		MonoBehaviour planeControls = (plane.GetComponent("ShipBehavior") as MonoBehaviour);
		planeControls.enabled = true;
		MonoBehaviour infopanels = (plane.GetComponent("InfoPanels") as MonoBehaviour);
		infopanels.enabled = true;
		MonoBehaviour boundries = (plane.GetComponent("KeepInBounds") as MonoBehaviour);
		boundries.enabled = true;
	}

	void SetLayerRecursively(GameObject go, int layerNumber){
		foreach (Transform trans in go.GetComponentsInChildren<Transform>(true)){
			trans.gameObject.layer = layerNumber;
		}
	}
	
}
