using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {
	
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private bool isShooting;
	
	void Update(){
		if (!photonView.isMine){
			transform.position = Vector3.Lerp (transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp (transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
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
			stream.SendNext (ShipBehavior.isShooting);
		}
		else{
			//Network Player, receive data
			this.correctPlayerPos = (Vector3) stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
			isShooting = (bool) stream.ReceiveNext();
			Debug.Log(this.isShooting);
		}
	}
}
