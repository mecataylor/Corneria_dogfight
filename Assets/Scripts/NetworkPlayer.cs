﻿using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {
	
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Vector3 correctPlayerVel;
	private bool isShooting;
	private bool shieldUp;
	
	void Update(){
		if (!photonView.isMine){
			transform.position = Vector3.Lerp (transform.position, correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp (transform.rotation, correctPlayerRot, Time.deltaTime * 5);
			rigidbody.velocity = correctPlayerVel;
			if(isShooting){
				transform.SendMessage("netShoot");
			}else{
				transform.SendMessage("netStopShoot");
			}
			if(shieldUp){
				gameObject.SendMessage("netShieldUp");
			}
		}
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			//We own this player: send the others our data
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
			stream.SendNext (rigidbody.velocity);
			stream.SendNext (ShipBehavior.isShooting);
			stream.SendNext (ShipBehavior.shieldOn);
		}
		else{
			//Network Player, receive data
			correctPlayerPos = (Vector3) stream.ReceiveNext();
			correctPlayerRot = (Quaternion) stream.ReceiveNext();
			correctPlayerVel = (Vector3) stream.ReceiveNext();
			isShooting = (bool) stream.ReceiveNext();
			shieldUp = (bool) stream.ReceiveNext();
		}
	}
}
