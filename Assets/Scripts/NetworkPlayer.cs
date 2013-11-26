using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {
	
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Vector3 correctPlayerVel;
	private bool isShooting;
	
	void Update(){
		if (!photonView.isMine){
			transform.position = Vector3.Lerp (transform.position, correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp (transform.rotation, correctPlayerRot, Time.deltaTime * 5);
			rigidbody.velocity = correctPlayerVel;
			if(isShooting){
				transform.SendMessage("startShooting");
			}else{
				transform.SendMessage("stopShooting");
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
		}
		else{
			//Network Player, receive data
			correctPlayerPos = (Vector3) stream.ReceiveNext();
			correctPlayerRot = (Quaternion) stream.ReceiveNext();
			correctPlayerVel = (Vector3) stream.ReceiveNext();
			isShooting = (bool) stream.ReceiveNext();
		}
	}
}
