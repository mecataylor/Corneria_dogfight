using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {
	
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Vector3 correctPlayerVel;
	private bool isShooting = false;
	private bool shieldUp = false;
	private bool died = false;
	
	void Update(){
		if (!photonView.isMine){
			transform.position = Vector3.Lerp (transform.position, this.correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp (transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
			rigidbody.velocity = correctPlayerVel;
			if(isShooting){
				transform.SendMessage("netShoot");
			}else{
				transform.SendMessage("netStopShoot");
			}
			if(shieldUp){
				gameObject.SendMessage("netShieldUp");
			}
			if(died){
				transform.SendMessage("netDied");
			}
		}
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			//We own this player: send the others our data
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
			stream.SendNext (rigidbody.velocity);
			stream.SendNext (isShooting);
			stream.SendNext (shieldUp);
			stream.SendNext (died);
			isShooting = false;
			shieldUp = false;
			died = false;
		}
		else{
			//Network Player, receive data
			correctPlayerPos = (Vector3) stream.ReceiveNext();
			correctPlayerRot = (Quaternion) stream.ReceiveNext();
			correctPlayerVel = (Vector3) stream.ReceiveNext();
			isShooting = (bool) stream.ReceiveNext();
			shieldUp = (bool) stream.ReceiveNext();
			died = (bool) stream.ReceiveNext();
		}
	}

	public void NetworkShoot(){
		isShooting = true;
	}

	public void NetworkShield(){
		shieldUp = true;
	}

	public void dead(){
		died = true;
	}
}
